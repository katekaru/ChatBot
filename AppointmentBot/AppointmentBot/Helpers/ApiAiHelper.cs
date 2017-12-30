using System.Linq;
using ApiAiSDK.Model;
using AppointmentBot.Enum;
using AppointmentBot.Model;

namespace AppointmentBot.Helpers
{
    public static class ApiAiHelper
    {
        public static ApiResponse ParseApiResponse(AIResponse resonse)
        {
            var result = new ApiResponse();
            if (resonse != null)
            {
                result.Action =
                    (ActionType)System.Enum.Parse(typeof(ActionType), resonse.Result?.Metadata?.IntentName);
                result.Parameters = resonse.Result?.HasParameters != null ? resonse.Result.Parameters : null;
                result.ResponseMessage = resonse.Result?.Fulfillment?.Speech;
                result.Score = resonse.Result.Score;
                result.Query = resonse.Result?.ResolvedQuery;
                result.SessionId = resonse.SessionId;
                result.IsComplete = !resonse.Result.Contexts.Any();
            }
            return result;

        }
    }
}