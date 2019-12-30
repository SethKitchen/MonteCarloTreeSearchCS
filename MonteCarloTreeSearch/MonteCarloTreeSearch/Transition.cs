using System;
using System.Collections.Generic;
using System.Text;

namespace MonteCarloTreeSearch
{
    /// <summary>
    /// Represents an segment between a <seealso cref="Node"/> of depth n and a child <seealso cref="Node"/> of depth n+1.
    /// Transitions MUST override <seealso cref="Object.equals(object)"/> and <seealso cref="Object.hashCode()"/>.
    /// <seealso cref="Transition"/> MIGHT represent a pass action. Implementations SHOULD take care of reducing
    /// memory footprint of this class (by keeping it stateless, using enums or whatever)
    /// 
    /// @author antoine vianey
    /// </summary>
    public interface ITransition
    {

    }
}
