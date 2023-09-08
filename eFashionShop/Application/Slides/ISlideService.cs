using eFashionShop.ViewModels.Utilities.Slides;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eFashionShop.Application.Slides
{
    public interface ISlideService
    {
        Task<List<SlideVm>> GetAll();
    }
}