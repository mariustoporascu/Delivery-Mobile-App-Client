using System;
using System.Collections.Generic;
using System.Text;

namespace FoodDeliveryApp.Constants
{
    public enum AuthOperations
    {
        Delete = 0,
        Login = 1,
        Create = 2,
        Profile = 3,
        Location = 4,
        SetPassword = 5,
        ChangePassword = 6,
        ResetPassword = 7,
        GenerateToken = 8,
        ConfirmEmail = 9,
    }
}
