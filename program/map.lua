scene = {
    cubo = {
        Transform = {
            position = " 0.000000 ,-0.500000 ,-0.000000 ",
            rotation = " 0 ,0 ,0 ",
            scale = " 1.000000 ,1.000000 ,1.000000 ",
        },
        RigidBody = {
            Type = "Static" ,
            Collider = "Box" ,
            Mass = "0.0" ,
            Dimensions = "10,  1.69,  10",
            isTrigger = "false" ,
            isEnabled = "true"
        },
        Enabled = "true",
        MeshRenderer = {
            MeshFile = "cubo.mesh",
        }
    },
    originalPieceMap = {
        Transform = {
            position = " -0.269432 ,0.727520 ,0.000000 ",
            rotation = " 0 ,0 ,0 ",
            scale = " 1.000000 ,1.000000 ,1.000000 ",
        },
        RigidBody = {
            Type = "Dynamic" ,
            Collider = "Box" ,
            Mass = "1.0" ,
            Dimensions = "1,  1,  1",
            isTrigger = "false" ,
            isEnabled = "true"
        },
        Enabled = "true",
        MeshRenderer = {
            MeshFile = "pieceMap.mesh",
        }
    },
    originalSphere = {
        Transform = {
            position = " -3.787061 ,2.838940 ,0.680828 ",
            rotation = " -22.582804 ,33.304256 ,-37.142292 ",
            scale = " 1.000000 ,1.000000 ,1.000000 ",
        },
        RigidBody = {
            Type = "Dynamic" ,
            Collider = "Sphere" ,
            Mass = "1.0" ,
            Dimensions = "1,  1,  1",
            isTrigger = "false" ,
            isEnabled = "true"
        },
        Enabled = "true",
        MeshRenderer = {
            MeshFile = "ball.mesh",
        }
    },
}

entities = {"cubo" ,"originalPieceMap" ,"originalSphere" }

