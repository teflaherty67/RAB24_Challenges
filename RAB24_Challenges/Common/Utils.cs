
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.DB.Plumbing;

namespace RAB24_Challenges.Common
{
    internal static class Utils
    {
        internal static RibbonPanel? CreateRibbonPanel(UIControlledApplication app, string tabName, string panelName)
        {
            RibbonPanel? curPanel;

            if (GetRibbonPanelByName(app, tabName, panelName) == null)
                curPanel = app.CreateRibbonPanel(tabName, panelName);

            else
                curPanel = GetRibbonPanelByName(app, tabName, panelName);

            return curPanel;
        }

        internal static Wall CreateWall(Document doc, Curve curve, WallType wt, Level level)
        {
            Wall curWall = Wall.Create(doc, curve, wt.Id, level.Id, 20, 0, false, false);

            return curWall;
        }

        internal static DuctType GetDuctTypeByName(Document doc, string ductType)
        {
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            collector.OfClass(typeof(DuctType));

            foreach (DuctType curDT in collector)
            {
                if (curDT.Name == ductType)
                    return curDT;
            }

            return null;
        }

        internal static FamilySymbol GetFamilySymbolByName(Document doc, string familyName, string typeName)
        {
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            collector.OfClass(typeof(FamilySymbol));

            foreach (FamilySymbol curFS in collector)
            {
                if (curFS.Name == typeName && curFS.FamilyName == familyName)
                    return curFS;
            }

            return null;
        }

        internal static Level GetLevelByName(Document doc, string levelName)
        {
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            collector.OfClass(typeof(Level));

            foreach (Level curLevel in collector)
            {
                if (curLevel.Name == levelName)
                    return curLevel;
            }

            return null;
        }

        internal static MEPSystemType GetMEPSystemTypeByName(Document doc, string mepSystemType)
        {
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            collector.OfClass(typeof(MEPSystemType));

            foreach (MEPSystemType curST in collector)
            {
                if (curST.Name == mepSystemType)
                    return curST;
            }

            return null;
        }

        internal static string GetParameterValueByName(Element element, string paramName)
        {
            IList<Parameter> paramList = element.GetParameters(paramName);

            if (paramList != null)
            {
                Parameter param = paramList[0];
                string paramValue = param.AsString();
                return paramValue;
            }

            return "";
        }

        internal static PipeType GetPipeTypeByName(Document doc, string pipeType)
        {
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            collector.OfClass(typeof(PipeType));

            foreach (PipeType curPT in collector)
            {
                if (curPT.Name == pipeType)
                    return curPT;
            }

            return null;
        }

        internal static RibbonPanel? GetRibbonPanelByName(UIControlledApplication app, string tabName, string panelName)
        {
            foreach (RibbonPanel tmpPanel in app.GetRibbonPanels(tabName))
            {
                if (tmpPanel.Name == panelName)
                    return tmpPanel;
            }

            return null;
        }

        internal static WallType GetWallTypeByName(Document doc, string wallType)
        {
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            collector.OfClass(typeof(WallType));

            foreach (WallType curWT in collector)
            {
                if (curWT.Name == wallType)
                    return curWT;
            }

            return null;
        }

        internal static void SetParameterByName(Element element, string paramName, int value)
        {
            IList<Parameter> paramList = element.GetParameters(paramName);

            if (paramList != null)
            {
                Parameter param = paramList[0];

                param.Set(value);
            }
        }
    }
}
