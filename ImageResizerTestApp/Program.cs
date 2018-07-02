using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using ImageSharp;
using ImageSharp.Formats;
using ImageSharp.Processing;
using ImageSharp.Processing.Processors;

namespace ImageResizerTestApp
{
    class Program
    {
        static void Main(string[] args)
        {
	        string[] files = Directory.GetFiles(args[0], "*.jpg");

	        Console.WriteLine($"Found {files.Length} files");
	        

			Parallel.ForEach(files, x =>
	        {
		        using (new AutoStopwatch($"Resizing {Path.GetFileName(x)}"))
		        {
			        string outputPath = Path.Combine(args[1], $"{Path.GetFileNameWithoutExtension(x)}.resized.jpg");
			        using (var input = File.OpenRead(x))
			        {
				        using (var output = File.OpenWrite(outputPath))
				        {
					        using (Image<Rgba32> image = Image.Load(input))
					        {
						        image.Resize(1024, 1024);
						        image.SaveAsJpeg(output);
					        }
					        output.Close();
				        }
				        input.Close();
			        }
		        }
	        });

        }
    }

	class AutoStopwatch : IDisposable
	{
		private Stopwatch mStopwatch;
		private string mMessage;


		public AutoStopwatch(string message)
		{
			mMessage = message;
			mStopwatch = new Stopwatch();
			mStopwatch.Start();
		}

		public void Dispose()
		{
			mStopwatch.Stop();
			Console.WriteLine($"Finsihed {mMessage} in {mStopwatch.Elapsed}");
		}
	}
}
