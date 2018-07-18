using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Firebase.Database;
using Firebase.Database.Query;

namespace AppointmentBot.Provider
{
    
    public class FirebaseProvider
    {
        public void SaveAppointment(string appt)
        {
            var auth = "AIzaSyCR7Z15zXyxaEQu1UrheIsq_ygzemImh84"; // your app secret
            var firebase = new FirebaseClient(
                "https://appointmentbot-ab967.firebaseio.com/");

            var appts = firebase.Child("appointments").PostAsync(appt).Result;
        }
    }
}
