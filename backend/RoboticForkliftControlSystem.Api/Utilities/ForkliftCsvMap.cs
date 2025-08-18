using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper.Configuration;
using RoboticForkliftControlSystem.Api.Entities;

namespace RoboticForkliftControlSystem.Api.Utilities;

public class ForkliftCsvMap : ClassMap<Forklift>
{
    public ForkliftCsvMap()
    {
        Map(m => m.Name).Name("name");
        Map(m => m.ModelNumber).Name("model_number");
        Map(m => m.ManufacturingDate).Name("manufacturing_date");
    }
}
