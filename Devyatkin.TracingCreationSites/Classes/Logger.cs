using Microsoft.SharePoint.Administration;
using System.Collections.Generic;

namespace Devyatkin.TracingCreationSites
{
    public class Logger : SPDiagnosticsServiceBase
    {
        public static string DiagnosticAreaName = "Devyatkin.TracingCreationSites";
        private static Logger _Current;
        public static Logger Current
        {
            get
            {
                if (_Current == null)
                {
                    _Current = new Logger();
                }

                return _Current;
            }
        }

        public Logger() : base("Devyatkin.TracingCreationSites Logging Service", SPFarm.Local)
        {

        }

        public enum Category
        {
            Unexpected,
            High,
            Medium,
            Information
        }

        protected override IEnumerable<SPDiagnosticsArea> ProvideAreas()
        {
            List<SPDiagnosticsArea> areas = new List<SPDiagnosticsArea>
        {
            new SPDiagnosticsArea(DiagnosticAreaName, new List<SPDiagnosticsCategory>
            {
                new SPDiagnosticsCategory("Unexpected", TraceSeverity.Unexpected, EventSeverity.Error),
                new SPDiagnosticsCategory("High", TraceSeverity.High, EventSeverity.Warning),
                new SPDiagnosticsCategory("Medium", TraceSeverity.Medium, EventSeverity.Information),
                new SPDiagnosticsCategory("Information", TraceSeverity.Verbose, EventSeverity.Information)
            })
        };

            return areas;
        }

        public static void WriteLog(Category categoryName, string source, string errorMessage)
        {
            SPDiagnosticsCategory category = Current.Areas[DiagnosticAreaName].Categories[categoryName.ToString()];
            Current.WriteTrace(0, category, category.TraceSeverity, string.Concat(source, ": ", errorMessage));
        }
    }
}
