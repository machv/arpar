using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Arpar
{
    /// <summary>
    /// Collection for saving arguments.
    /// </summary>
    public class Arguments : IEnumerable<Argument>
    {
        private List<Argument> arguments;

        #region Iterator definition
        public IEnumerator<Argument> GetEnumerator()
        {
            foreach (Argument argument in arguments)
            {
                yield return argument;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion

        public Arguments()
        {
            arguments = new List<Argument>();
        }

        public void Add(Argument argument)
        {
            arguments.Add(argument);
        }

        public void Remove(Argument argument)
        {
            arguments.Remove(argument);
        }

        public void Find(string name)
        {
            //najit argument v kolekci podle jmena -- pozor, hledat v nazvech vsech
        }
    }
}
