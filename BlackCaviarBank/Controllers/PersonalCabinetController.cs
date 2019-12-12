using BlackCaviarBank.Domain.Core;
using BlackCaviarBank.Domain.Interfaces;
using BlackCaviarBank.Infrastructure.Business;
using BlackCaviarBank.Infrastructure.Data;
using BlackCaviarBank.Infrastructure.Data.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlackCaviarBank.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class PersonalCabinetController : ControllerBase
    {
        private readonly UserManager<UserProfile> userManager;
        private readonly UnitOfWork unitOfWork;
        
        public PersonalCabinetController(UserManager<UserProfile> userManager, IUnitOfWork unitOfWork)
        {
            this.userManager = userManager;
            this.unitOfWork = (UnitOfWork)unitOfWork;
        }

        [HttpPut("ChangeProfileInfo")]
        public async Task<ActionResult> ChangeInfo(ProfileInfoDTO data)
        {
            if (string.IsNullOrEmpty(data.UserName))
            {
                ModelState.AddModelError("", "empty/null user name");
            }
            if (string.IsNullOrEmpty(data.Email))
            {
                ModelState.AddModelError("", "empty/null email");
            }
            if (string.IsNullOrEmpty(data.FirstName))
            {
                ModelState.AddModelError("", "empty/null user first name");
            }
            if (string.IsNullOrEmpty(data.LastName))
            {
                ModelState.AddModelError("", "empty/null user last name");
            }
            if (!new EmailAddressAttribute().IsValid(data.Email))
            {
                ModelState.AddModelError("", "invalid email");
            }

            if (ModelState.IsValid)
            {
                var currentUser = await userManager.GetUserAsync(User);

                currentUser.UserName = data.UserName.Equals("string") ? currentUser.UserName : data.UserName;
                currentUser.Email = data.Email.Equals("string") ? currentUser.Email : data.Email;
                currentUser.FirstName = data.FirstName.Equals("string") ? currentUser.FirstName : data.FirstName;
                currentUser.LastName = data.LastName.Equals("string") ? currentUser.LastName : data.LastName;

                var result = await userManager.UpdateAsync(currentUser);
                if (result.Succeeded)
                {
                    return Ok(currentUser);
                }
                else
                {
                    foreach(var error in result.Errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                    }

                    return Conflict(ModelState);
                }
            }
            else
            {
                return Conflict(ModelState);
            }
        }

        [HttpPut("ChangeProfileImage")]
        public async Task<ActionResult> ChangeImage(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                ModelState.AddModelError("", "null/empty image path");
            }

            if (ModelState.IsValid)
            {
                var user = await userManager.GetUserAsync(User);

                user.ProfileImage = System.IO.File.ReadAllBytes(path);
                
                unitOfWork.UserProfiles.Update(user);
                await unitOfWork.Save();

                return Ok(user.ProfileImage);
            }
            else
            {
                return Conflict(path);
            }
        }

        [HttpPut("ChangePassword")]
        public async Task<ActionResult> ChangePassword(ChangePasswordDTO data)
        {
            if (string.IsNullOrEmpty(data.OldPassword))
            {
                ModelState.AddModelError("", "null/empty old password");
            }
            if (string.IsNullOrEmpty(data.NewPassword))
            {
                ModelState.AddModelError("", "null/empty new password");
            }

            if (ModelState.IsValid)
            {
                var user = await userManager.GetUserAsync(User);

                var result = await userManager.ChangePasswordAsync(user, data.OldPassword, data.NewPassword);
                if (result.Succeeded)
                {
                    return Ok("Password has been changed successfully");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                    }

                    return Conflict(ModelState);
                }
            }
            else
            {
                return Conflict(ModelState);
            }
        }

        [HttpPost("ForgotPassword")]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordDTO data)
        {
            if (string.IsNullOrEmpty(data.Email))
            {
                ModelState.AddModelError("", "empty/null email");
            }
            if (!new EmailAddressAttribute().IsValid(data.Email))
            {
                ModelState.AddModelError("", "invalid email");
            }

            if (ModelState.IsValid)
            {
                var user = await userManager.GetUserAsync(User);

                var code = await userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.Action("ResetPassword", "PersonalCabinet", new { userId = user.Id, code }, protocol: HttpContext.Request.Scheme);
                EmailService emailService = new EmailService();
                await emailService.SendEmailAsync(data.Email, "Reset Password",
                    $"Для сброса пароля пройдите по ссылке: <a href='{callbackUrl}'>link</a>");
                
                return Ok($"An instruction has been sent on {data.Email}");
            }
            else
            {
                return Conflict(ModelState);
            }
        }

        [HttpPost("ResetPassword")]
        public async Task<ActionResult> ResetPassword(ResetPasswordDTO data)
        {
            if (string.IsNullOrEmpty(data.Email))
            {
                ModelState.AddModelError("", "null/empty email");
            }
            if (string.IsNullOrEmpty(data.Password))
            {
                ModelState.AddModelError("", "null/empty password");
            }
            if (string.IsNullOrEmpty(data.ConfirmPassword))
            {
                ModelState.AddModelError("", "null/empty confirm password");
            }
            if (string.IsNullOrEmpty(data.Code))
            {
                ModelState.AddModelError("", "null/empty code");
            }
            if (!data.ConfirmPassword.Equals(data.Password))
            {
                ModelState.AddModelError("", "confirm password is not equal to password");
            }
            if (!new EmailAddressAttribute().IsValid(data.Email))
            {
                ModelState.AddModelError("", "invalid email");
            }

            if (ModelState.IsValid)
            {
                var user = await userManager.GetUserAsync(User);
                
                var result = await userManager.ResetPasswordAsync(user, data.Code, data.Password);
                if (result.Succeeded)
                {
                    return Ok("Password has been reset");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                    }

                    return Conflict(ModelState);
                }
            }
            else
            {
                return Conflict(ModelState);
            }
        }

        [HttpPost("BlockCard")]
        public async Task<ActionResult> BlockCard(string cardNumber)
        {
            if (string.IsNullOrEmpty(cardNumber))
            {
                ModelState.AddModelError("", "null/empty card number");
            }
            if (cardNumber.Length != 16)
            {
                ModelState.AddModelError("", "card number length must be equal to 16");
            }

            if (ModelState.IsValid)
            {
                var user = await userManager.GetUserAsync(User);

                var card = unitOfWork.Cards.GetByNumberForUser(user, cardNumber);
                if (card != null)
                {
                    card.IsBlocked = true;

                    unitOfWork.Cards.Update(card);
                    await unitOfWork.Save();

                    return Ok($"card {card.CardNumber} has been blocked");
                }
                else
                {
                    return NotFound(cardNumber);
                }
            }
            else
            {
                return Conflict(ModelState);
            }
        }

        [HttpPost("UnblockCard")]
        public async Task<ActionResult> UnblockCard(string cardNumber)
        {
            if (string.IsNullOrEmpty(cardNumber))
            {
                ModelState.AddModelError("", "null/empty card number");
            }
            if (cardNumber.Length != 16)
            {
                ModelState.AddModelError("", "card number length must be equal to 16");
            }

            if (ModelState.IsValid)
            {
                var user = await userManager.GetUserAsync(User);

                var card = unitOfWork.Cards.GetByNumberForUser(user, cardNumber);
                if (card != null)
                {
                    card.IsBlocked = false;

                    unitOfWork.Cards.Update(card);
                    await unitOfWork.Save();

                    return Ok($"card {card.CardNumber} has been unblocked");
                }
                else
                {
                    return NotFound(cardNumber);
                }
            }
            else
            {
                return Conflict(ModelState);
            }
        }

        [HttpPost("BlockAccount")]
        public async Task<ActionResult> BlockAccount(string accountNumber)
        {
            if (string.IsNullOrEmpty(accountNumber))
            {
                ModelState.AddModelError("", "null/empty account number");
            }
            if (accountNumber.Length != 20)
            {
                ModelState.AddModelError("", "account number length must be equal to 20");
            }

            if (ModelState.IsValid)
            {
                var user = await userManager.GetUserAsync(User);

                var account = unitOfWork.Accounts.GetByNumberForUser(user, accountNumber);
                if (account != null)
                {
                    account.IsBlocked = true;

                    unitOfWork.Accounts.Update(account);
                    await unitOfWork.Save();

                    return Ok($"account {account.AccountNumber} has been blocked");
                }
                else
                {
                    return NotFound(accountNumber);
                }
            }
            else
            {
                return Conflict(ModelState);
            }
        }

        [HttpPost("UnblockAccount")]
        public async Task<ActionResult> UnblockAccount(string accountNumber)
        {
            if (string.IsNullOrEmpty(accountNumber))
            {
                ModelState.AddModelError("", "null/empty account number");
            }
            if (accountNumber.Length != 20)
            {
                ModelState.AddModelError("", "account number length must be equal to 20");
            }

            if (ModelState.IsValid)
            {
                var user = await userManager.GetUserAsync(User);

                var account = unitOfWork.Accounts.GetByNumberForUser(user, accountNumber);
                if (account != null)
                {
                    account.IsBlocked = false;

                    unitOfWork.Accounts.Update(account);
                    await unitOfWork.Save();

                    return Ok($"account {account.AccountNumber} has been unblocked");
                }
                else
                {
                    return NotFound(accountNumber);
                }
            }
            else
            {
                return Conflict(ModelState);
            }
        }

        [HttpGet("Contacts")]
        public async Task<ActionResult<List<UserProfile>>> GetContacts()
        {
            var user = await userManager.GetUserAsync(User);
            var src = unitOfWork.UserProfiles.GetWithContactsLoaded(user);

            var res = src.Contacts1.Where(c => c.IsApproved.Equals(true)).Concat(src.Contacts2.Where(c => c.IsApproved.Equals(true))).Distinct();
            var contacts = new List<UserProfile>();
            foreach(var doc in res)
            {
                contacts.Add(unitOfWork.UserProfiles.Get(doc.ReceiverId.Equals(user.Id) ? doc.SenderId : doc.ReceiverId));
            }
            return Ok(contacts);
        }

        [HttpGet("Contacts/{id}")]
        public async Task<ActionResult<UserProfile>> GetContact(string id)
        {
            var user = await userManager.GetUserAsync(User);
            var src = unitOfWork.UserProfiles.GetWithContactsLoaded(user);

            var res = src.Contacts1.Where(c => c.IsApproved.Equals(true)).Concat(src.Contacts2.Where(c => c.IsApproved.Equals(true))).Distinct();
            var contacts = new List<UserProfile>();
            foreach (var doc in res)
            {
                contacts.Add(unitOfWork.UserProfiles.Get(doc.ReceiverId.Equals(user.Id) ? doc.SenderId : doc.ReceiverId));
            }
            if (contacts.Contains(unitOfWork.UserProfiles.Get(id)))
            {
                return unitOfWork.UserProfiles.Get(id);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("ContactRequests")]
        public async Task<ActionResult<List<ContactRelationship>>> GetContactRequests()
        {
            var user = await userManager.GetUserAsync(User);
            var src = unitOfWork.UserProfiles.GetWithContactsLoaded(user);

            return Ok(src.Contacts2.Where(c => c.IsApproved.Equals(false) && c.SenderId != null && c.ReceiverId != null).ToList());
        }

        [HttpGet("ContactRequests/{id}")]
        public async Task<ActionResult<ContactRelationship>> GetContactRequest(int id)
        {
            var user = await userManager.GetUserAsync(User);
            var src = unitOfWork.UserProfiles.GetWithContactsLoaded(user);

            var res = src.Contacts2.FirstOrDefault(c => c.IsApproved.Equals(false) && c.SenderId != null && c.ReceiverId != null && c.ContactRelationshipId.Equals(id));
            if (res != null)
            {
                return Ok(res);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost("ApproveContactRequest")]
        public async Task<ActionResult<ContactRelationship>> ApproveContactRequest(int id)
        {
            var user = await userManager.GetUserAsync(User);
            var src = unitOfWork.UserProfiles.GetWithContactsLoaded(user);

            var req = src.Contacts2.FirstOrDefault(c => c.ContactRelationshipId.Equals(id) && c.IsApproved.Equals(false));
            if (req != null)
            {
                req.IsApproved = true;
                user.Contacts1.Add(req);
                unitOfWork.UserProfiles.Update(user);
                await unitOfWork.Save();
                return Ok(req);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost("AddContact")]
        public async Task<ActionResult<ContactRelationship>> AddContact(string userName)
        {
            var user = await userManager.GetUserAsync(User);

            var receiver = unitOfWork.UserProfiles.GetByUserName(userName);
            if (receiver != null)
            {
                var rel = new ContactRelationship { User1 = user, User2 = receiver };
                user.Contacts1.Add(rel);
                receiver.Contacts2.Add(rel);
                await unitOfWork.Save();
                return Ok(rel);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpDelete("RemoveContact/{id}")]
        public async Task<ActionResult> DeleteContact(int id)
        {
            var user = await userManager.GetUserAsync(User);
            var src = unitOfWork.UserProfiles.GetWithContactsLoaded(user);

            var req = src.Contacts1.FirstOrDefault(c => c.ContactRelationshipId.Equals(id));
            if (req != null)
            {
                req.IsApproved = false;
                user.Contacts1.Remove(req);
                unitOfWork.UserProfiles.Update(user);
                await unitOfWork.Save();
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }
    }
}