using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace RBA_Session_05_Challenge
{
    internal class FurnitureSet
    {
        public string Set { get; set; }
        public string Room { get; set; }
        public string[] Furniture { get; set; }
        public FurnitureSet(string _set, string _name, string _furnitureList)
        {
            Set = _set;
            Room = _name;
            Furniture = _furnitureList.Split(',');
        }

        public int GetFurnitureCount()
        {
            return Furniture.Length;
        }
    }

    internal class FurnitureType
    {
        public string Name { get; set; }
        public string Family { get; set; }
        public string Type { get; set; }
        public FurnitureType(string _name, string _family, string _type)
        {
            Name = _name;
            Family = _family;
            Type = _type;
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
    }

}