
using System.Diagnostics;

namespace Gianni.Helper
{
	public class SwapList // Value is Class  with Index
	{
		readonly int max;
		readonly SwapElement[] elements;
		ushort numActive_;

		public SwapList(SwapElement[] DisabledItems)
		{
			this.max = DisabledItems.Length;
			elements = DisabledItems;
			numActive_ = 0;
		}
		public void Activate(int index)
		{
			Debug.Assert(index >= numActive_);

			// Swap it with the first inactive particle
			// right after the active ones.
			SwapElement temp = elements[numActive_];
			//temp.IsAktive = true;
			elements[numActive_] = elements[index];
			elements[index] = temp;
			// Now there's one more.
			numActive_++;
		}
		public void Disable(ushort index)
		{
			Debug.Assert(index < numActive_);

			// There's one fewer.
			numActive_--;

			// Swap it with the last active particle
			// right before the inactive ones.
			SwapElement temp = elements[numActive_];
			elements[numActive_] = elements[index];
			
			// ADD INDEX VERY IMPORTEND TEMP OUT
			temp.ProjectilIndex = index;
			
			
			//elements[index].IsAktive = false;
			elements[index] = temp;
		}

		internal SwapElement Activate()
		{
			if (max == numActive_)
				return default;
			var ret = elements[numActive_];
			
			// ADD INDEX VERY IMPORTEND TEMP OUT
			ret.ProjectilIndex = numActive_++;
			
			
			return ret;
		}
		
	}
	public interface SwapElement
	{

		ushort ProjectilIndex
		{
			get;
			set;
		}
	}
}