﻿namespace DemoBackendArchitecture.Application.Interfaces;

public interface IEmailService
{
    void SendEmail(string to, string subject, string body);
}