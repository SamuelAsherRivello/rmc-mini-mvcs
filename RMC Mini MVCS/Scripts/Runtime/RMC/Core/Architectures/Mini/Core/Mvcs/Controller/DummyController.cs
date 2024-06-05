
using RMC.Core.Architectures.Mini.Model;
using RMC.Core.Architectures.Mini.Service;
using RMC.Core.Architectures.Mini.View;

namespace RMC.Core.Architectures.Mini.Controller
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