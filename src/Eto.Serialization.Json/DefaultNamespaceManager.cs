namespace Eto.Serialization.Json
{
	public class DefaultNamespaceManager : NamespaceManager
	{
		public DefaultNamespaceManager ()
		{
			var asm = typeof(Eto.Forms.Application).Assembly;
			DefaultNamespace = new NamespaceInfo("Eto.Forms", asm);
			Namespaces.Add ("drawing", new NamespaceInfo("Eto.Drawing", asm));
		}
	}
	
}
