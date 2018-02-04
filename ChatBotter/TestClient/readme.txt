This application is for tests of chatbotter
Syntax of client is described at ClientSyntaxFSATransitionTable.json

Entities: 
1. Project
2. User
3. Context
4. BotResponse
5. Filter

Commands for Project
1. Create // creates new project // Project -Create -Title "Project title" -Description "Project description"
2. Delete // deletes project by id // Project -Delete -Id "1"
3. Update // updates project // Project -Update -Id "1" [-Title "Project updated title"] [-Description updated "Project description"]
4. Use // to start use the project by id // Project -Use -Id "1"
5. CurrentInfo // info about current project // Project -CurrentInfo

Commands for User
1. Create // User -Create -Login "user_login" -Pass "user_password" -Name "user_name"
2. Delete // User -Delete -Id "123"
3. Update // User -Update -Id "123" [-Login "user_login"] [-Pass "user_password"] [-Name "user_name"]
4. Info // User -Info -Id "123"

Commands for Context
1. Create // Context -Create -Pattern "(A | B) & C | =\"D E F\" | (G /5 H)"
2. Delete // Context -Delete -Id "5"
3. Update // Context -Update -Id "5" -Pattern "A & B & C ~D"
4. Info // Context -Info -Id "5"

Commands for BotResponse
1. Create // BotResponse -Create -ContextId "5" -Text "Some bot answer..."
2. Delete // BotResponse -Delete -Id "10"
3. Update // BotResponse -Update -Id "10" [-ContextId "7"] [-Text "Some updated bot answer..."]
4. Info // BotResponse -Info -Id "10"

Commands for Filter
1. Create // Filter -Create -ContextId "33"
2. Delete // Filter -Delete -Id "9"
3. Update // Fileter -Update -ContextId "55"
4. Info // Filter -Info -Id "55"

Commands for Help
1. Help // Basic help of client info.
