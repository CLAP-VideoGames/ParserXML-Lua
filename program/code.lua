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