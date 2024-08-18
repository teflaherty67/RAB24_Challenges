using Autodesk.Revit.UI;

namespace RAB24_Challenges
{
    [Transaction(TransactionMode.Manual)]
    public class Module01_Challenge : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // this is a variable for the Revit application
            UIApplication uiapp = commandData.Application;

            // this is a variable for the current Revit model
            Document curDoc = uiapp.ActiveUIDocument.Document;

            // create some variables
            int numFloors = 250;
            double curElev = 0;
            double flrHeight = 15;
            int fizzCount = 0;
            int buzzCount = 0;
            int fizzbuzzCount = 0;

            // get titleblock type and element Id
            FilteredElementCollector colTB = new FilteredElementCollector(curDoc)
                .OfCategory(BuiltInCategory.OST_TitleBlocks)
                .WhereElementIsElementType();

            ElementId tblockId = colTB.FirstElementId();

            // get view family types
            FilteredElementCollector colVFT = new FilteredElementCollector(curDoc)
                .OfClass(typeof(ViewFamilyType));

            // create variables for view family types
            ViewFamilyType fpVFT = null;
            ViewFamilyType cpVFT = null;

            foreach (ViewFamilyType curVFT in colVFT)
            {
                if (curVFT.ViewFamily == ViewFamily.FloorPlan)
                {
                    fpVFT = curVFT;
                }
                else if (curVFT.ViewFamily == ViewFamily.CeilingPlan)
                {
                    cpVFT = curVFT;
                }
            }

            // create & start the transaction
            using (Transaction t = new Transaction(curDoc))
            {
                t.Start("FIZZ BUZZ Challenge");

                // loop through floors and check FIZZBUZZ
                for (int i = 1; i <= numFloors; i++)
                {
                    // create level for each number
                    Level newLevel = Level.Create(curDoc, curElev);
                    newLevel.Name = "LEVEL " + i.ToString();

                    // increment elevation by floor height
                    curElev += flrHeight;

                    // check for FIZZBUZZ
                    if (i % 3 == 0 && i % 5 == 0)
                    {
                        // if true, create sheet
                        ViewSheet newSheet = ViewSheet.Create(curDoc, colTB.FirstElementId());
                        newSheet.Name = "FIZZBUZZ_#" + i.ToString();
                        newSheet.SheetNumber = i.ToString();

                        ViewPlan newPlan = ViewPlan.Create(curDoc, fpVFT.Id, newLevel.Id);
                        newPlan.Name = "FIZZBUZZ_#" + i.ToString();

                        Viewport newVP = Viewport.Create(curDoc, newSheet.Id, newPlan.Id, new XYZ(1, 1, 0));

                        fizzbuzzCount++;
                     }                   
                    else if (i % 3 == 0)
                    {
                        // if true, create floor plan
                        ViewPlan newPlan = ViewPlan.Create(curDoc, fpVFT.Id, newLevel.Id);
                        newPlan.Name = "FIZZ_#" + i.ToString();

                        fizzCount++;
                    }                      
                    else if (i % 5 == 0)
                    {
                        // if true, create floor plan
                        ViewPlan newClgPlan = ViewPlan.Create(curDoc, cpVFT.Id, newLevel.Id);
                        newClgPlan.Name = "BUZZ_#" + i.ToString();

                        buzzCount++;
                    }
                }

                t.Commit();
            }
            
            // alert the user          
            TaskDialog.Show("Complete", $"Created {numFloors} levels. {fizzbuzzCount} FIZZBUZZ, {fizzCount} FIZZ, {buzzCount} BUZZ.");

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
