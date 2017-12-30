using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using ApiAiSDK;
using ApiAiSDK.Model;
using AppointmentBot.Enum;
using AppointmentBot.Helpers;
using AppointmentBot.Provider;
using Chronic;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using Appointment = AppointmentBot.Model.Appointment;


namespace AppointmentBot.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        private string SessionId;
        private Appointment appointment = new Appointment();

        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

        }
        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;

            var apiAiToken = ConfigurationManager.AppSettings["ApiAiToken"];
            var config = new AIConfiguration(apiAiToken, SupportedLanguage.English);
            if (!string.IsNullOrEmpty(SessionId))
            {
                config.SessionId = SessionId;
                appointment.AppointmentId = Guid.NewGuid();
            }
            var apiAi = new ApiAi(config);

            // return our reply to the user
            if (activity != null)
            {
                var response = ApiAiHelper.ParseApiResponse(apiAi.TextRequest(activity.Text));
                SessionId = response.SessionId;
                if (response.Parameters.All(x => string.IsNullOrEmpty(x.Value.ToString())))
                {
                    await context.PostAsync(response.ResponseMessage);
                    context.Wait(this.MessageReceivedAsync);
                    return;
                }

                foreach (var item in response.Parameters)
                {
                    switch (item.Key)
                    {
                        case "Datetime":
                            {
                                if (appointment.Datetime == DateTime.MinValue && item.Value != "")
                                {
                                    var data = item.Value.ToString();
                                    var errorMsg = string.Empty;
                                    var isError = false;
                                    DateTime date;
                                    if (!DateTime.TryParse(data, out date))
                                    {
                                        errorMsg = "Please Enter valid Appointment Date";
                                        isError = true;
                                    }
                                    else if (date.Date <= DateTime.Now.Date)
                                    {
                                        errorMsg = "Please Enter valid Appointment Date";
                                        isError = true;
                                    }
                                    else
                                    {
                                        appointment.Datetime = date;
                                        isError = false;
                                    }
                                    if (isError)
                                    {
                                        await context.PostAsync(errorMsg);
                                        context.Wait(this.MessageReceivedAsync);
                                        return;
                                    }
                                   
                                }
                            }
                            break;
                        case "Time":
                            {
                                if (appointment.Time == default(TimeSpan) && !string.IsNullOrEmpty(item.Value.ToString()))
                                {
                                    var data = item.Value.ToString();
                                    var errorMsg = string.Empty;
                                    TimeSpan time;
                                    var isError = false;
                                    if (!TimeSpan.TryParse(data, out time))
                                    {
                                        errorMsg = "Please Enter valid Appointment Time";
                                        isError = true;
                                    }
                                    else if (appointment.Datetime != DateTime.MinValue && time <= appointment.Datetime.TimeOfDay)
                                    {
                                        errorMsg = "Please Enter valid Appointment Time";
                                        isError = true;
                                    }
                                    else if (appointment.Datetime == DateTime.MinValue && time <= DateTime.Now.TimeOfDay)
                                    {
                                        errorMsg = "Please Enter valid Appointment Time";
                                        isError = true;
                                    }
                                    else
                                    {
                                        appointment.Time = time;
                                        isError = false;
                                    }
                                    if (isError)
                                    {
                                        await context.PostAsync(errorMsg);
                                        context.Wait(this.MessageReceivedAsync);
                                        return;
                                    }

                                }
                            }
                            break;
                        case "Make":
                            {
                                if (string.IsNullOrEmpty(appointment.Make) && !string.IsNullOrEmpty(item.Value.ToString()))
                                {
                                    appointment.Make = item.Value.ToString();

                                }

                            }
                            break;
                        case "Model":
                            {
                                if (string.IsNullOrEmpty(appointment.Model) && !string.IsNullOrEmpty(item.Value.ToString()))
                                {
                                    appointment.Model = item.Value.ToString();

                                }
                            ;
                            }
                            break;
                        case "Year":
                            {
                                if (string.IsNullOrEmpty(appointment.Year) && !string.IsNullOrEmpty(item.Value.ToString()))
                                {
                                    var data = item.Value.ToString();
                                    var errorMsg = string.Empty;
                                    var isError = false;
                                    Regex regex = new Regex(@"^\d{4}$");
                                    if (!regex.IsMatch(data))
                                    {
                                        errorMsg = "Please Enter valid Vehicle Year";
                                        isError = true;
                                    }
                                    else
                                    {
                                        appointment.Year = data;
                                        isError = false;
                                    }
                                    if (isError)
                                    {
                                        await context.PostAsync(errorMsg);
                                        context.Wait(this.MessageReceivedAsync);
                                        return;
                                    }
                                }

                            }
                            break;
                    }

                   
                }

                if (response.IsComplete)
                {
                    FirebaseProvider fp = new FirebaseProvider();
                    fp.SaveAppointment(JsonConvert.SerializeObject(appointment));
                }
                await context.PostAsync(response.ResponseMessage);
                context.Wait(this.MessageReceivedAsync);


            }

        }

     
    }
}