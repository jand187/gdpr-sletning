using System.Threading.Tasks;

namespace GdprClientConsole
{
	public interface IGdprCommand
	{
		Task Execute();
	}
}