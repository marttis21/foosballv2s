﻿using System;

namespace foosballv2s.Source.Services.CredentialStorage.Models
{
    public class Credential
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}