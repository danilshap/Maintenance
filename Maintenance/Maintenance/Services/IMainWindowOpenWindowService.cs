using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Maintenance.Controllers;
using Maintenance.Models;

namespace Maintenance.Services
{
    public interface IMainWindowOpenWindowService: IWindowOpenService {
        RepairOrder OpenAppendOrderWindow(DatabaseContext context);
        void OpenAppendWorkerWindow(Worker worker, DatabaseContext context);
        void OpenRequestWindow();
        void OpenAboutApplicationWindow();
    }
}
