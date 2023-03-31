using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

using CoH2XML2JSON.Blueprints;

namespace CoH2XML2JSON.Strategy.CoH3;

public class CoH3EntityReader : IBlueprintReader<EntityBlueprint> {
    public EntityBlueprint FromXml(XmlDocument xml, string modGuid, string filename, Helpers helpers) {
        throw new NotImplementedException();
    }
}
