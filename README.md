Phase 3:  
Incase View Anime Page doesn't work  
Go to Tools > NuGet Package Manager > Packeage Manager Console  
Type in these commands:  
'Add-Migration MergeAnimeDB -Context AnimeDbContext'  
'Update-Database -context AnimeDBContext'  

Phase 4:  
When you create a new anime with an image, you will probably see the view anime site broken again but there should be a apply mirgation button on the site, click it and refresh it should work.

Phase 5:
Incase Browse Anime Page doesn't work  
Go to Tools > NuGet Package Manager > Packeage Manager Console  
Type in these commands:    
'Update-Database -context AnimeDBContext'  
  
If that does not work try this instead:
'Add-Migration AddAnimeReactions -Context AnimeDbContext'  
then 'Update-Database -context AnimeDBContext' 