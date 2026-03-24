using FuenfzehnZeitWrapper.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FuenfzehnZeitWrapper.Application.Services;

public sealed class FuenfzehnZeitService : IFuenfzehnZeitService
{
    public Task LogInAsync()
    {
        return Task.CompletedTask;
    }
}
