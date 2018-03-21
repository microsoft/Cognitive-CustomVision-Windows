
# Prediction
Write-Host "Generating Predicton API files"
AutoRest -Namespace "Microsoft.Cognitive.CustomVision.Prediction" -Input "Microsoft.Cognitive.CustomVision.Prediction\Prediction-swagger.json" -AddCredentials -SyncMethods All -OutputDirectory "Microsoft.Cognitive.CustomVision.Prediction\Generated" -CodeGenerator "CSharp"

Write-Host "Generating Training API files"
AutoRest -Namespace "Microsoft.Cognitive.CustomVision.Training" -Input "Microsoft.Cognitive.CustomVision.Training\Training-swagger.json" -AddCredentials -SyncMethods All -OutputDirectory "Microsoft.Cognitive.CustomVision.Training\Generated" -CodeGenerator "CSharp"
