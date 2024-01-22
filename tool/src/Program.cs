using System.CommandLine;

var rootCommand = new RootCommand("Open Minecraft Players Tool for interacting with players list data");
rootCommand.AddCommand(CreateHumanizeCommand());
rootCommand.AddCommand(CreateCheckCommand());
rootCommand.AddCommand(CreateAddCommand());

await rootCommand.InvokeAsync(args);

static Command CreateHumanizeCommand()
{
    var fileArgument = new Argument<FileInfo>("file", "Players list file path").ExistingOnly();

    var outputOption = new Option<FileInfo>(["--output", "-o"], "Output text file path") { IsRequired = true };
    outputOption.LegalFileNamesOnly();

    var command = new Command("humanize", "Convert player list from binary data to text file");
    command.AddArgument(fileArgument);
    command.AddOption(outputOption);
    command.SetHandler(
        async (file, output) =>
        {
            await using var fromFile = file.OpenRead();
            using var reader = new BinaryReader(fromFile);

            await using var toFile = output.Create();
            using var writer = new StreamWriter(toFile);

            while (fromFile.Position < fromFile.Length)
            {
                var guid = new Guid(reader.ReadBytes(16));
                writer.WriteLine(guid.ToString());
            }
        },
        fileArgument,
        outputOption);

    return command;
}

static Command CreateCheckCommand()
{
    var uuidArgument = new Argument<Guid>("uuid", "Target UUID");

    var fileOption = new Option<FileInfo>(["--file", "-f"], "Players list file path") { IsRequired = true };
    fileOption.ExistingOnly();

    var command = new Command("check", "Check if players list contains target UUID");
    command.AddArgument(uuidArgument);
    command.AddOption(fileOption);
    command.SetHandler(
        async (targetGuid, file) =>
        {
            await using var fileStream = file.OpenRead();
            using var reader = new BinaryReader(fileStream);

            while (fileStream.Position < fileStream.Length)
            {
                var guid = new Guid(reader.ReadBytes(16));
                
                if (guid == targetGuid)
                {
                    Console.WriteLine("found");
                    return;
                }
            }

            Console.WriteLine("not found");
        },
        uuidArgument,
        fileOption);

    return command;
}

static Command CreateAddCommand()
{
    var uuidArgument = new Argument<Guid>("uuid", "UUID to add");

    var fileOption = new Option<FileInfo>(["--file", "-f"], "Players list file path") { IsRequired = true };
    fileOption.ExistingOnly();

    var command = new Command("add", "Add specified UUID to the players list");
    command.AddArgument(uuidArgument);
    command.AddOption(fileOption);
    command.SetHandler(
        async (guidToAdd, file) =>
        {
            await using var fileStream = file.Open(FileMode.Append);
            using var writer = new BinaryWriter(fileStream);

            writer.Write(guidToAdd.ToByteArray());
        },
        uuidArgument,
        fileOption);

    return command;
}
