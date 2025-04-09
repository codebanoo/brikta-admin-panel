using APIs.Automation.Models.Business;
using APIs.Core.Controllers;
using APIs.CustomAttributes;
using APIs.CustomAttributes.Helper;
using APIs.Melkavan.Models.Business;
using APIs.Projects.Models.Business;
using APIs.Public.Models.Business;
using APIs.Public.Models.Entities;
using APIs.TelegramBot.Models.Business;
using APIs.Teniaco.Models.Business;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Models.Business.ConsoleBusiness;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using VM.Base;
using VM.Console;
using VM.Public;
using VM.PVM.Melkavan;

namespace APIs.Melkavan.Controllers
{
    /// <summary>
    /// MelkavanLoginManagementController
    /// </summary>
    [CustomApiAuthentication]
    public class MelkavanLoginManagementController : ApiBaseController
    {
        /// <summary>
        /// BuildingLifesManagementController
        /// </summary>
        /// <param name="_hostingEnvironment"></param>
        /// <param name="_httpContextAccessor"></param>
        /// <param name="_actionContextAccessor"></param>
        /// <param name="_configurationRoot"></param>
        /// <param name="_consoleBusiness"></param>
        /// <param name="_automationApiBusiness"></param>
        /// <param name="_publicApiBusiness"></param>
        /// <param name="_teniacoApiBusiness"></param>
        /// <param name="_melkavanApiBusiness"></param>
        /// <param name="_projectsApiBusiness"></param>
        /// <param name="_telegramBotApiBusiness"></param>
        /// <param name="_appSettingsSection"></param>
        public MelkavanLoginManagementController(IHostEnvironment _hostingEnvironment,
            IHttpContextAccessor _httpContextAccessor,
            IActionContextAccessor _actionContextAccessor,
            IConfigurationRoot _configurationRoot,
            IConsoleBusiness _consoleBusiness,
            IAutomationApiBusiness _automationApiBusiness,
            IPublicApiBusiness _publicApiBusiness,
            ITeniacoApiBusiness _teniacoApiBusiness,
            IMelkavanApiBusiness _melkavanApiBusiness,
            IProjectsApiBusiness _projectsApiBusiness,
            ITelegramBotApiBusiness _telegramBotApiBusiness,
            IOptions<AppSettings> _appSettingsSection) :
            base(
                _hostingEnvironment,
                _httpContextAccessor,
                _actionContextAccessor,
                _configurationRoot,
                _consoleBusiness,
                _automationApiBusiness,
                _publicApiBusiness,
                _teniacoApiBusiness,
                _melkavanApiBusiness,
                _projectsApiBusiness,
                _telegramBotApiBusiness,
                _appSettingsSection)
        {

        }



        /// <summary>
        /// SendSMSservice
        /// </summary>
        /// <param name="melkavanLoginPVM"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>

        [HttpPost("SendSMSservice")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult SendSMSservice([FromBody] MelkavanLoginPVM
            melkavanLoginPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });


            try
            {


                #region SendSMSservice

                SmsSendersVM smsSendersVM = new SmsSendersVM();
                SendSmsService sendSmsService = new SendSmsService();

                //یک کد رندوم ساخته میشود
                var verifyCode = sendSmsService.RandomCode();

                // به پنل پیامکی سفیر آنلاین وصل میشود
                var smsSender = publicApiBusiness.PublicApiDb.SmsSenders.Where(c => c.IsDefault == true && c.IsActivated.Value.Equals(true) && c.IsDeleted.Value.Equals(false)).FirstOrDefault();

                if (smsSender != null)
                {
                    // اگر رندوم کد خالی نبود
                    if (!string.IsNullOrEmpty(verifyCode))
                    {
                        {
                            var sendSmsResult = sendSmsService.SendSms(melkavanLoginPVM.MelkavanLoginVM.UserName, verifyCode, smsSender);
                            // اگر اتصال به پنل پیامکی اوکی بود
                            if (sendSmsResult != null)
                            {

                                melkavanLoginPVM.TempVerifyCode = verifyCode;

                                jsonResultWithRecordObjectVM.Result = "OK";
                                jsonResultWithRecordObjectVM.Record = ComputeSha256Hash(melkavanLoginPVM.TempVerifyCode);
                                jsonResultWithRecordObjectVM.Value = melkavanLoginPVM.MelkavanLoginVM.UserName;
                            }
                        }

                    }
                    else
                    {
                        jsonResultWithRecordObjectVM.Result = "ERROR";
                        jsonResultWithRecordObjectVM.Message = "ErrorInService";
                    }


                }
                #endregion

            }
            catch (Exception exc)
            { }

            return new JsonResult(jsonResultWithRecordObjectVM);
        }


        #region SendSmsService

        public class ConfirmCodeViewModel
        {
            public string UserName { get; set; }
            public string VirificationCode { get; set; }
            public bool Result { get; set; }
        }

        public class SendSmsService
        {
            public string RandomCode()
            {
                Random rd = new Random();
                int randomNumber = rd.Next(1000, 9999);
                return randomNumber.ToString();
            }
            public string SendSms(string phoneNumber, string verifyCode, SmsSenders smsSender)
            {
                if (!string.IsNullOrWhiteSpace(phoneNumber))
                {
                    var request = (HttpWebRequest)WebRequest.Create("http://sms.safironline.ir/webservice/rest/sms_send?");

                    var MobileNumber = phoneNumber;
                    var postData = "note_arr= کد تایید کاربر ملکاوان : " + verifyCode + "&login_username=" + smsSender.UserName + "& login_password=" + smsSender.Password + "&receiver_number=" + MobileNumber + "&sender_number=" + smsSender.SmsSenderNumber;
                    //var postData = "login_username=teniaco&login_password=Avoi2@1400&receiver_number=09122060766&note_arr[]=سلام&sender_number=500021163";

                    var data = Encoding.UTF8.GetBytes(postData);

                    request.Method = "POST";
                    request.ContentType = "application/x-www-form-urlencoded";
                    request.ContentLength = data.Length;

                    using (var stream = request.GetRequestStream())
                    {
                        stream.Write(data, 0, data.Length);
                    }

                    var response = (HttpWebResponse)request.GetResponse();

                    var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

                    var MsgStat = GetEnumFromJsonString(responseString);
                    return MsgStat.ToString();
                }
                else
                {
                    return "لطفا شماره موبایل را وارد کنید";
                }
            }
            public static Enum GetEnumFromJsonString(String jsonStr)
            {
                return ConvertStatusCodeToEnum(GetStatusCode(GetListStrJson(jsonStr)));
            }

            private static String GetListStrJson(String JsonInpVal)
            {
                try
                {
                    if (JsonConvert.DeserializeObject<JsonModel>(JsonInpVal).list[0] == null)
                        return JsonConvert.DeserializeObject<JsonModel>(JsonInpVal).result.ToString();
                    else
                        return JsonConvert.DeserializeObject<JsonModel>(JsonInpVal).list[0];

                }
                catch (Exception ex)
                {
                    //Save exception log:
                    throw new Exception(ex.Message);
                }
            }

            private static int GetStatusCode(String listInput)
            {
                var StrArray = listInput.Split('_');
                return int.Parse(StrArray[StrArray.Length - 1]);
            }

            private static Enum ConvertStatusCodeToEnum(int inpCodeStatus)
            {
                return (Conditions)inpCodeStatus;
            }
        }
        enum Conditions
        {
            NotFound = -1,
            WithoutStatus = 0,
            Success = 1,
            NotArrived = 2,
            NotSendToCommunication = 3,
            SendToCommunication = 4,
            CommunicationBlockList = 5,
            SendToOperator = 8,
            CombackCost = 9,
            SendQueue = 10,
            Spam = 11,
            Expire = 12,
            WithoutRoot = 13,
            Rejected = 14,
            NotArrivedToCommunication = 16,
            WithoutDeliveryStatus = 17,

        }
        public class JsonModel
        {
            public Boolean result { get; set; }
            public String[] list { get; set; }
        }

        #endregion

        #region ConvertToHash

        public static string ComputeSha256Hash(string input)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // Convert the input string to a byte array and compute the hash.
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

                // Convert the byte array to a hexadecimal string.
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        #endregion



        /// <summary>
        /// MelkavanLogin
        /// </summary>
        /// <param name="melkavanLoginPVM"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>

        [HttpPost("MelkavanLogin")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult MelkavanLogin([FromBody] MelkavanLoginPVM
            melkavanLoginPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (melkavanLoginPVM.ChildsUsersIds == null)
                    {
                        melkavanLoginPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (melkavanLoginPVM.ChildsUsersIds.Count == 0)
                        melkavanLoginPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (melkavanLoginPVM.ChildsUsersIds.Count == 1)
                        if (melkavanLoginPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            melkavanLoginPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);

                }


                var melkavanLoginVM = melkavanLoginPVM.MelkavanLoginVM;

                long login = melkavanApiBusiness.MelkavanLogin(
                    ref melkavanLoginVM,
                    consoleBusiness,
                    publicApiBusiness);

                if (login.Equals(-1))
                {
                    jsonResultWithRecordObjectVM.Result = "ERROR";
                    jsonResultWithRecordObjectVM.Message = "ErrorLogin";
                }
                else if (login > 0)
                {

                    var user = consoleBusiness.CmsDb.Users.Where(f => f.UserId.Equals(login)).FirstOrDefault();

                    melkavanLoginPVM.MelkavanLoginVM.CustomUsersVM = new CustomUsersVM();
                    melkavanLoginPVM.MelkavanLoginVM.CustomUsersVM.UserId = login;
                    melkavanLoginPVM.MelkavanLoginVM.CustomUsersVM.UserName = user.UserName;
                    melkavanLoginPVM.MelkavanLoginVM.CustomUsersVM.DomainSettingId = user.DomainSettingId;


                    #region Check The Levels And Roles for the Login User

                    var userProfile = consoleBusiness.CmsDb.UsersProfile.Where(f => f.Mobile.Equals(melkavanLoginPVM.MelkavanLoginVM.UserName)).FirstOrDefault();

                    var levelIds = consoleBusiness.GetMultiLevelsUserWithUserId(userProfile.UserId).LevelIds;
                    var levelNames = consoleBusiness.GetLevelsWithLevelIds(levelIds).Select(l => l.LevelName);


                    //اگر دسترسی  مالک یا واسط نداشته باشد
                    if (levelNames != null)
                    {
                        if (levelNames.Contains("مالک") || levelNames.Contains("مشاور"))
                        {
                            if (user != null)
                            {
                                userId = user.UserId;
                                if (user.IsActivated.HasValue && user.IsDeleted.HasValue)
                                    if (user.IsActivated.Value && !user.IsDeleted.Value)
                                    {
                                        var roles = consoleBusiness.GetRolesWithUserId(user.UserId);
                                        if (roles.Count() > 0)
                                        {
                                            if (roles.Count() == 1)
                                            {
                                                if (!roles.Any(r => r.RoleName.Equals("Hosts") ||
                                                                    r.RoleName.Equals("Admins") ||
                                                                    r.RoleName.Equals("Users")))
                                                {
                                                    jsonResultWithRecordObjectVM.Result = "ERROR";
                                                    jsonResultWithRecordObjectVM.Message = "ErrorLogin";
                                                }

                                                string indexUrl = "";

                                                if (roles.Any(r => r.RoleName.Equals("Admins")))
                                                {
                                                    indexUrl = "/admin/adminPanel/Index";
                                                }
                                                else if (roles.Any(r => r.RoleName.Equals("Users")))
                                                {
                                                    indexUrl = "/users/usersPanel/Index";
                                                    if (levelNames.Contains("سرمایه گذاران"))
                                                    {
                                                        indexUrl = "/UserTeniaco/TeniacoPanel/Index";
                                                    }
                                                    else if (levelNames.Contains("نماینده"))
                                                    {
                                                        indexUrl = "/UserTeniaco/TeniacoPanel/Index";
                                                    }
                                                }
                                                else if (roles.Any(r => r.RoleName.Equals("Hosts")))
                                                {
                                                    indexUrl = "/host/hostPanel/Index";
                                                }


                                                roleIds = roles.Select(r => r.RoleId).ToList();

                                                userName = user.UserName;
                                                var levelsDetails = consoleBusiness.GetLevelsDetailsWithUserIdAndRoleIds(user.UserId, roleIds);
                                                levelIds = levelsDetails.Select(l => l.LevelId).ToList();
                                                var levelNames2 = string.Join(",", levelsDetails.Select(l => l.LevelName).ToArray());
                                                roleIds = roles.Select(r => r.RoleId).ToList();
                                                roleNames = string.Join(",", roles.Select(l => l.RoleName).ToArray());


                                                jsonResultWithRecordObjectVM.Result = "OK";
                                                jsonResultWithRecordObjectVM.Message = "LoginOk";
                                                jsonResultWithRecordObjectVM.Record = melkavanLoginPVM.MelkavanLoginVM.CustomUsersVM;
                                                jsonResultWithRecordObjectVM.ContentType = indexUrl;
                                            }
                                            else if (roles.Count() > 1)
                                            {
                                                if (!roles.Any(r => r.RoleName.Equals("Hosts") ||
                                                                   r.RoleName.Equals("Admins") ||
                                                                   r.RoleName.Equals("Users")))
                                                {
                                                    jsonResultWithRecordObjectVM.Result = "ERROR";
                                                    jsonResultWithRecordObjectVM.Message = "ErrorLogin";
                                                }

                                                string indexUrl = "";


                                                
                                                if (roles.Any(r => r.RoleName.Contains("Users")))
                                                {
                                                    indexUrl = "/users/usersPanel/Index";
                                                    if (levelNames.Contains("سرمایه گذاران"))
                                                    {
                                                        indexUrl = "/UserTeniaco/TeniacoPanel/Index";
                                                    }
                                                    else if (levelNames.Contains("نماینده"))
                                                    {
                                                        indexUrl = "/UserTeniaco/TeniacoPanel/Index";
                                                    }
                                                }
                                                else if (roles.Any(r => r.RoleName.Equals("Hosts")))
                                                {
                                                    indexUrl = "/host/hostPanel/Index";
                                                }


                                                roleIds = roles.Select(r => r.RoleId).ToList();

                                                userName = user.UserName;
                                                var levelsDetails = consoleBusiness.GetLevelsDetailsWithUserIdAndRoleIds(user.UserId, roleIds);
                                                levelIds = levelsDetails.Select(l => l.LevelId).ToList();
                                                var levelNames2 = string.Join(",", levelsDetails.Select(l => l.LevelName).ToArray());
                                                roleIds = roles.Select(r => r.RoleId).ToList();
                                                roleNames = string.Join(",", roles.Select(l => l.RoleName).ToArray());


                                                jsonResultWithRecordObjectVM.Result = "OK";
                                                jsonResultWithRecordObjectVM.Message = "LoginOk";
                                                jsonResultWithRecordObjectVM.Record = melkavanLoginPVM.MelkavanLoginVM.CustomUsersVM;
                                                jsonResultWithRecordObjectVM.ContentType = indexUrl;

                                            }




                                        }
                                      
                                    }
                            }


                        }
                    }
                    else
                    {
                        jsonResultWithRecordObjectVM.Result = "ERROR";
                        jsonResultWithRecordObjectVM.Message = "ErrorLogin";
                    }



                    #endregion


                }


                return new JsonResult(jsonResultWithRecordObjectVM);
            }
            catch (Exception exc)
            { }

            jsonResultWithRecordObjectVM.Result = "ERROR";
            jsonResultWithRecordObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordObjectVM);
        }



    }



}
