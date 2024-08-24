using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.DB.Plumbing;
using RAB24_Challenges.Common;
using System.Windows.Controls;

namespace RAB24_Challenges
{
    [Transaction(TransactionMode.Manual)]
    public class Module02_Challenge : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // this is a variable for the Revit application
            UIApplication uiapp = commandData.Application;

            // this is a variable for the current Revit model
            Document curDoc = uiapp.ActiveUIDocument.Document;

            // this is a variable for the current Revit model in the UI
            UIDocument uidoc = uiapp.ActiveUIDocument;

            // prompt user to select elements
            TaskDialog.Show("Select lines", "Select lines to convert to Revit elements.");
            IList<Element> pickList = uidoc.Selection.PickElementsByRectangle("Select elements");

            // filter the selected elements
            List<CurveElement> filteredList = new List<CurveElement>();

            foreach (Element curElem in pickList)
            {
                if (curElem is CurveElement)
                {
                    CurveElement curCurve = curElem as CurveElement;
                    filteredList.Add(curCurve);
                }
            }

            // notify the user
            TaskDialog.Show("Curves", $"You selected + {filteredList.Count} + curves.");

            // get the level
            Parameter levelParam = curDoc.ActiveView.LookupParameter("Associated Level");
            Level curLevel = Utils.GetLevelByName(curDoc, levelParam.AsString());

            // get types by name
            WallType wallType01 = Utils.GetWallTypeByName(curDoc, "Storefront");
            WallType wallType02 = Utils.GetWallTypeByName(curDoc, "Generic - 8\"");

            MEPSystemType pipeSystemType = Utils.GetMEPSystemTypeByName(curDoc, "Domestic Hot Water");
            PipeType pipeType = Utils.GetPipeTypeByName(curDoc, "Default");

            MEPSystemType ductSystemType = Utils.GetMEPSystemTypeByName(curDoc, "Supply Air");
            DuctType ductType = Utils.GetDuctTypeByName(curDoc, "Default");

            // create list for lines to lhide
            List<ElementId> linesToHide = new List<ElementId>();

            // transaction
            using (Transaction t = new Transaction(curDoc))
            {
                t.Start("Reveal Message");

                // loop through curves and create elements






            }



                return Result.Succeeded;
        }
        internal static PushButtonData GetButtonData()
        {
            // use this method to define the properties for this command in the Revit ribbon
            string buttonInternalName = "btnCommand1";
            string buttonTitle = "Button 1";
            string? methodBase = MethodBase.GetCurrentMethod().DeclaringType?.FullName;

            if (methodBase == null)
            {
                throw new InvalidOperationException("MethodBase.GetCurrentMethod().DeclaringType?.FullName is null");
            }
            else
            {
                Common.ButtonDataClass myButtonData1 = new Common.ButtonDataClass(
                    buttonInternalName,
                    buttonTitle,
                    methodBase,
                    Properties.Resources.Blue_32,
                    Properties.Resources.Blue_16,
                    "This is a tooltip for Button 1");

                return myButtonData1.Data;
            }
        }
    }

}
