scene = {
    cubo = {
        Transform = {
            position = " -3.500000 ,1.500000 ,-0.000000 ",
            rotation = " 0 ,0 ,-44.999958 ",
            scale = " 1.000000 ,1.000000 ,1.000000 ",
        },
        RigidBody = {
            Type = "Dynamic",
             Collider = "Box",
             Mass = "1.0",
            isTrigger = "false",
             isEnabled = "true"
        },
        Enabled = "true",
        MeshRenderer = {
            MeshFile = "cubo.mesh",
        }
    },
    suelo = {
        Transform = {
            position = " -3.500000 ,-0.500000 ,0.000000 ",
            rotation = " 0 ,0 ,0 ",
            scale = " 1.000000 ,1.000000 ,1.000000 ",
        },
        RigidBody = {
            Type = "Static",
             Collider = "Box",
             Mass = "1.0",
             isTrigger = "false",
             isEnabled = "true"
        },
        Enabled = "true",
        MeshRenderer = {
            MeshFile = "suelo.mesh",
        }
    },
    detalle = {
        Transform = {
            position = " -2.322678 ,0.085155 ,0.000000 ",
            rotation = " 0 ,0 ,0 ",
            scale = " 1.000000 ,1.000000 ,1.000000 ",
        },
        RigidBody = {
            Type = "Static",
             Collider = "Box",
             Mass = "1.0",
             isTrigger = "false",
             isEnabled = "true"
        },
        Enabled = "false",
        MeshRenderer = {
            MeshFile = "detalle.mesh",
        }
    },
}

entities = {"cubo" ,"suelo" ,"detalle" }

--[[ code to be executed once the script is registered ]]--
local s = ScriptManager.getInstance();

player = {
    name = "Francisquito",
    position = { x = 1.90, y = 0.8}
}
s:createPlayerbyObject(scene.cubo);

function sumPosition()
    player.position.x = player.position.x + 10;
end

sumPosition();
