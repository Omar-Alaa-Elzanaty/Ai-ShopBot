using System;
using System.Collections.Generic;
using System.Text;

namespace Ai_ShopBot.Application.Features.Auth.LoginWithRefreshToken
{
    public class LoginWithRefreshTokenDto
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
