using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Voracious.EPub.Extensions;

public static class XmlExt
{
    public static List<string> AsStringList(this IEnumerable<XElement> self)
    {
        return self.Select(elem => elem.Value).ToList();
    }

    public static List<T> AsObjectList<T>(this IEnumerable<XElement> self, Func<XElement, T> factory)
    {
        return self.Select(factory).Where(value => value != null).ToList();
    }
}
