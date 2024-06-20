using RMC.Mini.Model;
using RMC.Mini.Service;
using RMC.Mini.View;

//Keep As:RMC.Mini.Controller
namespace RMC.Mini.Controller
{
    /// <summary>
    /// Optional. Useful when you want to use 'no concern' here,
    /// but your setup requires one.
    /// </summary>
    public class DummyController : BaseController<DummyModel, DummyView, DummyService>
    {
        public DummyController(DummyModel model, DummyView view, DummyService service) 
            : base(model, view, service)
        {
        }
    }
}