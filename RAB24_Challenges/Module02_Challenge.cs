using Autodesk.Revit.DB;
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
                if (curElem is CurveElement curCurve)
                    filteredList.Add(curCurve);
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
                foreach (CurveElement curCurve in filteredList)
                {
                    // get Curve and GraphicStyle of each CurveElement
                    Curve curveElem = curCurve.GeometryCurve;
                    GraphicsStyle curGS = curCurve.LineStyle as GraphicsStyle;

                    // filter out lines to hide
                    if (curGS.Name != "A-GLAZ" && curGS.Name != "A-WALL" &&
                        curGS.Name != "M-DUCT" && curGS.Name != "P-PIPE")
                    {
                        linesToHide.Add(curCurve.Id);
                        continue;
                    }

                    // get start and end points
                    XYZ startPoint = curveElem.GetEndPoint(0);
                    XYZ endPoint = curveElem.GetEndPoint(1);

                    // create wall, duct or pipe
                    switch(curGS.Name)
                    {
                        case "A-GLAZ":
                            Wall newWall1 = Utils.CreateWall(curDoc, curveElem, wallType01, curLevel);
                            break;

                        case "A-WALL":
                            Wall newWall2 = Utils.CreateWall(curDoc, curveElem, wallType02, curLevel);
                            break;

                        case "M-DUCT":
                            Duct newDuct = Duct.Create(curDoc, ductSystemType.Id, ductType.Id,
                                curLevel.Id, startPoint, endPoint);
                            break;

                        case "P-PIPE":
                            Pipe newPipe = Pipe.Create(curDoc, pipeSystemType.Id, pipeType.Id,
                                curLevel.Id, startPoint, endPoint);
                            break;

                        default:
                            break;
                    }

                }

                // hide lines
                curDoc.ActiveView.HideElements(linesToHide);

                t.Commit();
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
