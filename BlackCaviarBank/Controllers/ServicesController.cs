using AutoMapper;
using BlackCaviarBank.Domain.Core;
using BlackCaviarBank.Domain.Interfaces;
using BlackCaviarBank.Infrastructure.Data;
using BlackCaviarBank.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlackCaviarBank.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        private readonly UserManager<UserProfile> userManager;
        private readonly UnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IOperation operation;

        public ServicesController(UserManager<UserProfile> userManager, IUnitOfWork unitOfWork, IMapper mapper, IOperation operation)
        {
            this.userManager = userManager;
            this.unitOfWork = (UnitOfWork)unitOfWork;
            this.mapper = mapper;
            this.operation = operation;
        }

        [Authorize]
        [HttpGet]
        public ActionResult<List<Service>> GetAllServices() => unitOfWork.Services.GetAll().ToList();

        [Authorize(Roles = "admin")]
        [HttpGet("{id}")]
        public ActionResult<Service> GetService(int id)
        {
            var result = unitOfWork.Services.Get(id);

            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return NotFound($"there is no service with id {id}");
            }
        }

        [Authorize]
        [HttpGet("ForCurrentUser")]
        public async Task<ActionResult<List<Service>>> GetAllForCurrentUser()
        {
            var user = await userManager.GetUserAsync(User);

            if (user != null)
            {
                return unitOfWork.Services.GetAllForUser(user).ToList();
            }
            else
            {
                return BadRequest("There is no authenticated user");
            }
        }

        /// <remarks>
        /// Sample request:
        ///
        ///     POST /RegisterService
        ///     {
        ///        "name": "JotaroKujoDreams",
        ///        "price": 100000
        ///     }
        ///
        /// </remarks>
        [Authorize(Roles = "admin")]
        [HttpPost("RegisterService")]
        public async Task<ActionResult<Service>> CreateService(ServiceDTO data)
        {
            if (string.IsNullOrEmpty(data.Name))
            {
                ModelState.AddModelError("name", "Service name must not be empty");
            }

            if (ModelState.IsValid)
            {
                var service = mapper.Map<Service>(data);

                unitOfWork.Services.Create(service);

                await unitOfWork.Save();

                return CreatedAtAction(nameof(GetService), new { id = service.ServiceId }, service);
            }
            else
            {
                return Conflict(ModelState);
            }
        }

        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /UpdateService
        ///     {
        ///        "name": "JotaroKujoDreams",
        ///        "price": 100000
        ///     }
        ///
        /// </remarks>
        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult<Service>> UpdateService(ServiceDTO data, int id)
        {
            if (string.IsNullOrEmpty(data.Name))
            {
                ModelState.AddModelError("name", "Service name must not be empty");
            }
            if (data.Price < 0)
            {
                ModelState.AddModelError("price", "Service price must not be less than 0");
            }

            if (ModelState.IsValid)
            {
                var service = unitOfWork.Services.Get(id);
                service.Name = data.Name.Equals("string") ? service.Name : data.Name;
                service.Price = data.Price.Equals(0) ? service.Price : data.Price;

                unitOfWork.Services.Update(service);
                await unitOfWork.Save();

                return CreatedAtAction(nameof(GetService), new { id = service.ServiceId }, service);
            }
            else
            {
                return Conflict(ModelState);
            }
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> RemoveService(int id)
        {
            var target = unitOfWork.Services.Get(id);
            string name = target.Name;

            unitOfWork.Services.Delete(id);

            await unitOfWork.Save();

            return Ok($"Service with name ${name} has been removed");
        }

        /// <remarks>
        /// Sample request:
        ///
        ///     POST /SubscribeOnService
        ///     {
        ///        "cardNumber": "1111222233334444",
        ///        "serviceId": 1
        ///     }
        ///
        /// </remarks>
        [Authorize]
        [HttpPost("SubscribeOnService")]
        public async Task<ActionResult> SubcribeOnService(SubscriptionDTO data)
        {
            var user = await userManager.GetUserAsync(User);

            if (user != null)
            {
                if (string.IsNullOrEmpty(data.CardNumber))
                {
                    ModelState.AddModelError("cardNumber", "card number must not be empty");
                }

                if (ModelState.IsValid)
                {
                    var card = unitOfWork.Cards.GetByNumberForUser(user, data.CardNumber);

                    if (card != null)
                    {
                        var service = unitOfWork.Services.Get(data.ServiceId);

                        if (service != null)
                        {
                            var subscription = new SubscriptionSubscriber { Subscriber = user, Subscription = service };
                            if (operation.PayForSubscription(card, service))
                            {
                                user.SubscriptionSubscribers.Add(subscription);
                                service.SubscriptionSubscribers.Add(subscription);
                                await unitOfWork.Save();
                                return Ok($"subscribed successfully on service {service.Name}");
                            }
                            else
                            {
                                return BadRequest($"not enough money on card {card.CardNumber} to subcribe on service {service.Name}");
                            }
                        }
                        else
                        {
                            return NotFound($"there is no service with id {data.ServiceId}");
                        }
                    }
                    else
                    {
                        return NotFound($"there is no card with number {data.CardNumber}");
                    }
                }
                else
                {
                    return Conflict(ModelState);
                }
            }
            else
            {
                return BadRequest("There is no authenticated user");
            }
        }

        [Authorize]
        [HttpDelete]
        public async Task<ActionResult> UnsubscribeFromService(string serviceName)
        {
            var user = await userManager.GetUserAsync(User);

            var service = unitOfWork.Services.GetByName(serviceName);
            if(service != null)
            {
                var subscription = user.SubscriptionSubscribers.FirstOrDefault(ss => ss.SubscriptionId.Equals(service.ServiceId));
                if (subscription != null)
                {
                    user.SubscriptionSubscribers.Remove(subscription);
                    service.SubscriptionSubscribers.Remove(subscription);
                    await unitOfWork.Save();
                    return Ok($"Subscription on {service} has been cancelled");
                }
                else
                {
                    return BadRequest($"You did not subscribe on service {serviceName}");
                }
            }
            else
            {
                return NotFound();
            }
        }
    }
}