using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CoH2XML2JSON.Blueprints;

namespace CoH2XML2JSON.Strategy.Listeners;

public class NullListener : IStrategyListener {
    public void Notify<T>(IList<T> blueprints) where T : IBlueprint {}
}
