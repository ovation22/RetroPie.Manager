# A RetroPie Manager Project in Blazor

[![.NET](https://github.com/ovation22/RetroPie.Manager/actions/workflows/dotnet.yml/badge.svg)](https://github.com/ovation22/RetroPie.Manager/actions/workflows/dotnet.yml)
![GitHub license](https://img.shields.io/github/license/hiulit/cross-compile-godot-raspberry-pi?style=flat-square)

RetroPie Manager is a web-based interface for managing and configuring your RetroPie setup. This tool aims to simplify the management of games, controllers, and system settings for RetroPie users.

This is very much still a work in progress.

## Features

- Web-Based Interface: Access RetroPie Manager through a web browser, making it convenient to manage your RetroPie setup from any device on your network.
- Game Management: Easily add, remove, or organize your game collections directly from the web interface.
- Controller Configuration: Configure and manage controllers, ensuring a seamless gaming experience.
- System Settings: Access and modify RetroPie system settings without the need for a keyboard and mouse connected to your Raspberry Pi.

### Included in the solution

- [x] Display CPU usage
- [x] Display Disk usage
- [x] Display Memory usage
- [x] View list of ROMs
- [x] View the Emulation Station log file
- [ ] Edit the Emulation Station config file
- [ ] Edit the RetroArch config file
- [ ] Edit the autostart.sh script
- [ ] Manage your BIOS files
- [ ] Manage your ROMS
- [ ] Set to run the server automatically on start
- [ ] Support file renaming
- [ ] Support for manage splash screens
- [ ] Support for sub directories
- [ ] Support for Systems with multiple directories (Like Mame)
- [ ] Support for moving/copying files


## Getting Started

### Prerequisites

- RetroPie installed on your Raspberry Pi or other device.

### Installation

1. Install .NET 8

```bash
wget -O - https://raw.githubusercontent.com/pjgpetecodes/dotnet8pi/main/install.sh | sudo bash
```

2. Install git

```bash
sudo apt install git
```

3. Clone the Repository:

```bash
git clone https://github.com/ovation22/RetroPie.Manager.git
```

4. Navigate to the Project Directory:

```bash
cd RetroPie.Manager
```

5. Run the Application:

```bash
dotnet run --project ./src/RetroPie.Manager.Web/RetroPie.Manager.Web.csproj --urls http://0.0.0.0:5000
``` 

5. Access RetroPie Manager:

Open your web browser and navigate to http://your-pi-ip:5000, replacing your-pi-ip with the IP address of your Raspberry Pi.

## Links

- https://retropie.org.uk/
- https://github.com/RetroPie/RetroPie-Manager
- https://github.com/fechy/retropie-web-gui
- https://fluentui-blazor.net/

## Give a Star! :star:

If you like the project, please consider giving it a star!

## Contributing

Any and all are welcome to contribute to this project.
Please read our [Contributing Guidelines](/.github/CONTRIBUTING.md)

## Code of Conduct

Any and all are welcome to contribute to this project.
Please read our [Code of Conduct](/.github/CODE_OF_CONDUCT.md)

## Acknowledgments

Special thanks to the [RetroPie](https://retropie.org.uk/) community for their support and contributions.
