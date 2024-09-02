using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.DB.Plumbing;
using RAB24_Challenges.Common;
using System.Windows.Controls;
using Forms = System.Windows.Forms;
using Excel = OfficeOpenXml;
using OfficeOpenXml;
using RBA_Session_05_Challenge;
using Autodesk.Revit.DB.Architecture;

namespace RAB24_Challenges
{
    [Transaction(TransactionMode.Manual)]
    public class Module03_Challenge : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // this is a variable for the Revit application
            UIApplication uiapp = commandData.Application;

            // this is a variable for the current Revit model in the UI
            UIDocument uidoc = uiapp.ActiveUIDocument;

            // this is a variable for the current Revit model
            Document curDoc = uiapp.ActiveUIDocument.Document;
            
            // promt user to select Excel file
            Forms.OpenFileDialog selectFile = new Forms.OpenFileDialog();
            selectFile.Filter = "Excel files|*.xls;*.xlsx;*.xlsm";
            selectFile.InitialDirectory = "S:\\";
            selectFile.Multiselect = false;

            string excelFile = "";

            // check to see if selected file is an Excel file
            if (selectFile.ShowDialog() == Forms.DialogResult.OK)
                excelFile = selectFile.FileName;

            if (excelFile == "")
            {
                TaskDialog.Show("Error", "Please select an Excel file.");
                return Result.Failed;
            }

            // set the EPPlus license context
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            // open the Excel file
            ExcelPackage excel = new ExcelPackage(excelFile);
            ExcelWorkbook workbook = excel.Workbook;
            ExcelWorksheet setWS = workbook.Worksheets[0];
            ExcelWorksheet typeWS = workbook.Worksheets[1];

            // get row and column count
            int setRows = setWS.Dimension.Rows;
            int typeRows = typeWS.Dimension.Rows;

            // put data into list of classes
            List<FurnitureType> furnTypeList = new List<FurnitureType>();
            List<FurnitureSet> furnSetList = new List<FurnitureSet>();

            for (int i = 1; i <= setRows; i++)
            {
                string setName = setWS.Cells[i, 1].Value.ToString();
                string setRoom = setWS.Cells[i, 2].Value.ToString();
                string setFurn = setWS.Cells[i, 3].Value.ToString();

                FurnitureSet curSet = new FurnitureSet(setName, setRoom, setFurn);
                furnSetList.Add(curSet);
            }

            for (int j = 1; j <= typeRows; j++)
            {
                string typeName = typeWS.Cells[j, 1].Value.ToString();
                string typeFamily = typeWS.Cells[j, 2].Value.ToString();
                string typeType = typeWS.Cells[j, 3].Value.ToString();

                FurnitureType curType = new FurnitureType(typeName, typeFamily, typeType);
                furnTypeList.Add(curType);
            }

            // remove the column headers
            furnTypeList.RemoveAt(0);
            furnSetList.RemoveAt(0);

            int overallCounter = 0;

            // get all the rooms
            FilteredElementCollector colRooms = new FilteredElementCollector(curDoc);
            colRooms.OfCategory(BuiltInCategory.OST_Rooms);

            // create & start a transaction
            using (Transaction t = new Transaction(curDoc, "Insert Furniture"))
            {
                t.Start();

                // loop through the rooms & insert furniture
                foreach (SpatialElement curRoom in colRooms)
                {
                    int counter = 0;

                    // get the required furniture set
                    string furnSet = Utils.GetParameterValueByName(curRoom, "Furniture Set");

                    // get the insertion point (room location point)
                    LocationPoint roomLocation = curRoom.Location as LocationPoint;
                    XYZ insPoint = roomLocation.Point;


                }
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
