namespace K6502Emu
{
	public interface IAddressable<TDataSize>
	{
		/// <summary>
		/// Indexers for reading from or writing to an addressable at the specified address.
		/// </summary>
		/// <param name="address">Address at which to read from or write to.</param>
		/// <returns>The data to read from the specified address.</returns>
		public TDataSize this[int address] { get; set; }

		/// <summary>
		/// A property used for determining the maximum address allowed by or set for the <see cref="IAddressable{TDataSize}"/>.
		/// This is more intended to be used by code outside an object implementing this interface rather than the code inside the implementation of that object.
		/// </summary>
		public int AddressableSize { get; }

		public bool IsReadonly { get; set; }
	}
}
