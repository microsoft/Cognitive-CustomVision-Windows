
# Prediction
Write-Host "Generating Predicton API files"
AutoRest --version 1.0.1-20170508-2300-nightly -Namespace "Microsoft.Cognitive.CustomVision" -Input "Microsoft.Cognitive.CustomVision.Prediction\Prediction-swagger.json" -AddCredentials -SyncMethods All -OutputDirectory "Microsoft.Cognitive.CustomVision.Prediction\Generated" -CodeGenerator "CSharp"

Write-Host "Generating Training API files"
AutoRest --version 1.0.1-20170508-2300-nightly -Namespace "Microsoft.Cognitive.CustomVision" -Input "Microsoft.Cognitive.CustomVision.Training\Training-swagger.json" -AddCredentials -SyncMethods All -OutputDirectory "Microsoft.Cognitive.CustomVision.Training\Generated" -CodeGenerator "CSharp"
