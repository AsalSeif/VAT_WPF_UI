# VAT_WPF_UI
This is a small WPF UI that calculate Valued-added tax on your purchases. At the current time calculation for 4 countries are supported:Austria, Singapore, United Kingdom and Portugal. Calculation base on VAT, Price without VAT and Price with VAT are ready to use :).
Calculation service is implemented as a web api, So to use this aaplication, first you need to clone and run web api [here](https://github.com/AsalSeif/VAT_Calculation_API/blob/main/README.md), then start this project.

## Getting Started

Clone the repository.
```shell
git clone https://github.com/AsalSeif/VAT_WPF_UI.git
```

### Run application

1. Start application
```shell
cd VAT_WPF_UI\WpfTaxApp\WpfTaxApp\bin\Debug
start WpfTaxApp.exe
```


### How to use
After run the application, supported countries are loaded in combobox on top of the window and you should select your country.
The available tax rates will be shown and you can select one!
The system provides 3 types of calculation :
1. Base on "Price without VAT" : if you like to calculate VAT and price with VAT (Gross) amounts for your purchases.
2. Base on "Value-Added Tax" : If you know VAT amount and like to calculate price without VAT (Net) and with VAT (Gross).
3. Base on :Price with VAT" : If you know Price with VAT (Gross) and like to calculate VAT and price with VAT (Net).

You should select the type of calculation and then enter the right amount according the selected type and press Enter key.
The other 2 missing values in the window will be calculate and shown:).

![image](https://user-images.githubusercontent.com/58383289/233681152-7f6a1f0c-b832-4afd-a814-65a1d7da33c5.png)


### Possible errors
ClientSide Errors:

    Error Code 200: VAT Rate is not valid.
    Error Code 201: Price or VAT value is Zero or Negative!!!
    Error Code 202: Country is not supported!!!
    Error Code 203: VAT Rate does not specified!!!
    Error Code 204: Server is not reachable or Response is not Ok.
    
ServerSide Errors:

    Error Code 100:  Tax Rate is not valid.
    Erroe Code 101:  Price or VAT value is Zero or Negative!!!
    Error Code 102:  Country is not supported!!!
