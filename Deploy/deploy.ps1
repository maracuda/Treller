properties {
  $base_dir = Resolve-Path .
  $remote_server = "192.168.52.58"
  $install_path = "\\$remote_server\C$\Treller"
  $install_path_local = "C:\Treller"
  $iis_app_pool_name = "Treller"
  $iis_app_pool_dotnet = "v4.5"
  $iis_site_name = "Treller.App"
}

task default -depends SetAppOnline

task Build {
    exec { dotnet restore "$base_dir\..\IoCContainer" }
    exec { dotnet restore "$base_dir\..\Infrastructure" }
    exec { dotnet restore "$base_dir\..\HttpInfrastructure" }
    exec { dotnet restore "$base_dir\..\Serialization" }
    exec { dotnet restore "$base_dir\..\Storage" }
    exec { dotnet restore "$base_dir\..\TaskManagerClient" }
    exec { dotnet restore "$base_dir\..\WebApplication" }
    exec { dotnet build "$base_dir\..\WebApplication" -c Release }
}

task SetAppOffline -depends Build {
    if (Test-Path $install_path -pathType container)
    {
        Copy-Item "$base_dir\App_Offline.htm" $install_path -Force
        Copy-Item "$base_dir\web.config" $install_path -Force
        if (Test-Path "$install_path\global.asax") { Remove-Item "$install_path\global.asax" -Force }
        Start-Sleep -s 10
    }
}

task Clean -depends SetAppOffline {
    if (Test-Path $install_path -pathType container)
    {
        Remove-Item "$install_path\*" -Exclude "App_Offline.htm","web.config" -Force -Recurse
        Remove-Item "$install_path\Views" -Force -Recurse
        Start-Sleep -s 10
    }
}

task CopyContent -depends Clean {
    if (!(Test-Path $install_path -pathType container))
    {
        New-Item $install_path -Type Directory
    }
    Copy-Item "$base_dir\..\WebApplication\bin\Release\net461\*" "$install_path\bin" -Recurse -Force
    Copy-Item "$base_dir\..\WebApplication\Content" "$install_path\Content" -Recurse -Force
    Copy-Item "$base_dir\..\WebApplication\fonts" "$install_path\fonts" -Recurse -Force
    Copy-Item "$base_dir\..\WebApplication\Scripts" "$install_path\Scripts" -Recurse -Force
    Copy-Item "$base_dir\..\WebApplication\Views" "$install_path\Views" -Recurse -Force
    Copy-Item "$base_dir\..\WebApplication\Global.asax" "$install_path\Global.asax" -Recurse -Force
    Copy-Item "$base_dir\..\WebApplication\Web.config" "$install_path\Web.config" -Recurse -Force
}

task CreateWebsite -depends CopyContent {
    [Reflection.Assembly]::LoadWithPartialName('Microsoft.Web.Administration')
    $server_manager = [Microsoft.Web.Administration.ServerManager]::OpenRemote($remote_server)
    if (!$server_manager.ApplicationPools[$iis_app_pool_name])
    {
        $iis_app_pool = $server_manager.ApplicationPools.Add($iis_app_pool_name)
        $iis_app_pool.ManagedRuntimeVersion = $iis_app_pool_dotnet
        $server_manager.CommitChanges()
        }
    if (!$server_manager.Sites[$iis_site_name])
    {
        $iis_site = $server_manager.Sites.Add($iis_site_name, $install_path_local,  80)
        $iis_site.ServerAutoStart = True
        $iis_site.Start()
        $server_manager.CommitChanges()
    }
}

task SetAppOnline -depends CreateWebsite {
    if (Test-Path "$install_path\App_Offline.htm")
    {
        Remove-Item "$install_path\App_Offline.htm" -Force
    }
}