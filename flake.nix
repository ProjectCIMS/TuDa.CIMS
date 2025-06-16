{
  description = "Flake for a .NET application on NixOS with ICU support";

  inputs = {
    nixpkgs.url = "github:NixOS/nixpkgs/nixos-unstable";
    flake-utils.url = "github:numtide/flake-utils";
  };

  outputs = { self, nixpkgs, flake-utils, ... }:
    flake-utils.lib.eachDefaultSystem (system:
      let
        pkgs = import nixpkgs { inherit system; };
      in {
        # A development shell that sets LD_LIBRARY_PATH appropriately.
        devShells.default = pkgs.mkShell {
          buildInputs = [ pkgs.dotnet-sdk_9 pkgs.icu ];
          shellHook = ''
            # For using nix-ld
            export NIX_LD_LIBRARY_PATH=${pkgs.icu}/lib:$NIX_LD_LIBRARY_PATH
            # For not using nix-ld
            export LD_LIBRARY_PATH=${pkgs.icu}/lib:$LD_LIBRARY_PATH
          '';
        };
      }
    );
}

