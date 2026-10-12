$apiUrl = "http://localhost:5041/api/products" # Change if your API uses a different URL

$products = @(
    @{ name = "Wireless Mouse"; price = 29.99 },
    @{ name = "Mechanical Keyboard"; price = 79.99 },
    @{ name = "USB-C Hub"; price = 39.50 }
)

foreach ($product in $products) {
    $json = $product | ConvertTo-Json

    try {
        $result = Invoke-RestMethod `
            -Uri $apiUrl `
            -Method Post `
            -ContentType "application/json" `
            -Body $json `
            -ErrorAction Stop

        Write-Host "Added: $($product.name)"
    }
    catch {
        Write-Error "Failed to add '$($product.name)': $_"
    }
}