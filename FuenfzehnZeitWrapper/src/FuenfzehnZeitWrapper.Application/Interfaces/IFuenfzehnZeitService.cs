using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace FuenfzehnZeitWrapper.Application.Interfaces
{
    public interface IFuenfzehnZeitService
    {
        Task LogInAsync();
    }
}
