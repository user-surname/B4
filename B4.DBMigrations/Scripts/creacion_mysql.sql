USE slava_db;


CREATE TABLE IF NOT EXISTS CONTROL (

	idControl INT NOT NULL,
	Anyo INT NOT NULL,
	idCiclo INT NOT NULL,
	idFaseControl INT NOT NULL,
	Inicio DATETIME NOT NULL,
	Final DATETIME NOT NULL,
	Activo BOOLEAN NULL,
	AddInfo VARCHAR(4000) NULL,
	SendAutEmail BOOLEAN NULL,
	BWReportsMandatory BOOLEAN NULL,
 PRIMARY KEY (idControl )
);

CREATE TABLE IF NOT EXISTS LK_PLANT_COMPANY (

	IdCompany INT NOT NULL,
	CompanyCode VARCHAR(50) NOT NULL,
	ManagementCompany VARCHAR(100) NOT NULL,
	idCurrency INT NOT NULL,
	Company VARCHAR(10) NOT NULL,
	Active BOOLEAN NULL,
	idDivision INT NOT NULL,
	idDivisionCompany INT NOT NULL,
	idSubdivision INT NOT NULL,
	idCountry INT NOT NULL,
	Location VARCHAR(150) NOT NULL,
	obs VARCHAR(4000) NULL,
	RegionalValidatorPwd VARCHAR(50) NULL,
	Region VARCHAR(150) NULL,
	RegionalValidator VARCHAR(150) NULL,
	RegionalValidatorEmail VARCHAR(250) NULL,
	ContributorPwd VARCHAR(50) NULL,
	ManagementCompanyBackup VARCHAR(100) NULL,
	RegionBackup VARCHAR(50) NULL,
	CountryBackup VARCHAR(100) NULL,
	idRegValidator INT NULL,
 PRIMARY KEY (IdCompany )
);

CREATE TABLE IF NOT EXISTS LK_EPIGRAFES (

	idEpigrafe INT NOT NULL,
	idplantilla INT NOT NULL,
	idHoja INT NOT NULL,
	PreEpigrafe VARCHAR(255) NULL,
	Epigrafe VARCHAR(255) NOT NULL,
	EpigrafeFull VARCHAR(255) NOT NULL,
 PRIMARY KEY (idEpigrafe )
);

CREATE TABLE IF NOT EXISTS CONTROL_PLANTA (

	idControlPlanta INT NOT NULL,
	idControl INT NOT NULL,
	idCompany INT NOT NULL,
 PRIMARY KEY (idControlPlanta )
);

CREATE TABLE IF NOT EXISTS LK_CICLOS (

	id INT AUTO_INCREMENT NOT NULL,
	idCiclo INT NOT NULL,
	Ciclo VARCHAR(50) NOT NULL,
	Descripcion VARCHAR(50) NOT NULL,
 PRIMARY KEY (id)
);

CREATE TABLE IF NOT EXISTS LK_FASES (

	idFase INT NOT NULL,
	Fase VARCHAR(50) NOT NULL,
	FaseAlias VARCHAR(10) NULL,
 PRIMARY KEY (idFase )
);

CREATE TABLE IF NOT EXISTS LK_PLANT_COUNTRY (

	idCountry INT NOT NULL,
	Country VARCHAR(100) NOT NULL,
 PRIMARY KEY (idCountry )
);

CREATE TABLE IF NOT EXISTS LK_PLANT_SUBDIVISION (

	idSubdivision INT NOT NULL,
	Subdivision VARCHAR(100) NOT NULL,
 PRIMARY KEY (idSubdivision )
);

CREATE TABLE IF NOT EXISTS LK_PLANT_DIVISION_COMPANY (

	idDivisionCompany INT NOT NULL,
	DivisionCompany VARCHAR(100) NULL,
 PRIMARY KEY (idDivisionCompany )
);
CREATE TABLE IF NOT EXISTS LK_PLANT_DIVISION (

	idDivision INT NOT NULL,
	Division VARCHAR(100) NOT NULL,
 PRIMARY KEY (idDivision )
);

CREATE TABLE IF NOT EXISTS LK_PLANT_CURRENCY (

	idCurrency INT NOT NULL,
	Currency VARCHAR(50) NOT NULL,
	CurrencyAlias VARCHAR(10) NOT NULL,
 PRIMARY KEY (idCurrency )
);

CREATE OR REPLACE VIEW V_API_CONTROL
AS
SELECT
	CONTROL.idcontrol,
	CONTROL.Anyo,
	CONTROL.idciclo,
	LK_CICLOS.ciclo as Ciclo,
	LK_CICLOS.Descripcion as descCiclo,
	CONTROL.inicio,
	CONTROL.final,
	CONTROL.idFaseControl as idFase,
	LK_FASES.FaseAlias as codFase,
	LK_FASES.Fase as descFase,
	CONTROL.Activo
FROM
	CONTROL 
	LEFT JOIN LK_CICLOS 
		ON CONTROL.idciclo = LK_CICLOS.idciclo
	LEFT JOIN LK_FASES 
		ON CONTROL.idFaseControl = LK_FASES.idFase;


CREATE TABLE IF NOT EXISTS DATA_Comentarios (

	Id INT AUTO_INCREMENT NOT NULL,
	IdAPICarga INT NOT NULL,
	guidCarga CHAR(36) NOT NULL,
	FechaUltModif DATETIME NULL,
	IdCompany INT NOT NULL,
	Ejercicio INT NOT NULL,
	IdCiclo INT NOT NULL,
	IdFase INT NOT NULL,
	IdEpigrafe INT NOT NULL,
	Etiqueta VARCHAR(50) NOT NULL,
	Comentario TEXT NULL,
 	PRIMARY KEY (Id)
);

CREATE TABLE IF NOT EXISTS DATA_Forecast (

	id INT AUTO_INCREMENT NOT NULL,
	idAPICarga INT NOT NULL,
	guidCarga CHAR(36) NOT NULL,
	FechaUltModif DATETIME NOT NULL,
	idCompany INT NOT NULL,
	ejercicio INT NOT NULL,
	idCiclo INT NOT NULL,
	idFase INT NOT NULL,
	idCurrency INT NOT NULL,
	idEpigrafe INT NOT NULL,
	mes00 DECIMAL(18,5) NOT NULL,
	mes01 DECIMAL(18,5) NOT NULL,
	mes02 DECIMAL(18,5) NOT NULL,
	mes03 DECIMAL(18,5) NOT NULL,
	mes04 DECIMAL(18,5) NOT NULL,
	mes05 DECIMAL(18,5) NOT NULL,
	mes06 DECIMAL(18,5) NOT NULL,
	mes07 DECIMAL(18,5) NOT NULL,
	mes08 DECIMAL(18,5) NOT NULL,
	mes09 DECIMAL(18,5) NOT NULL,
	mes10 DECIMAL(18,5) NOT NULL,
	mes11 DECIMAL(18,5) NOT NULL,
	mes12 DECIMAL(18,5) NOT NULL,
	mes13 DECIMAL(18,5) NOT NULL,
PRIMARY KEY (id)
);

CREATE TABLE IF NOT EXISTS DATA_BRIDGESFY_BW_EUR (

	Id INT AUTO_INCREMENT NOT NULL,
	idAPICarga INT NOT NULL,
	guidCarga CHAR(36) NULL,
	FechaUltModif DATETIME NOT NULL,
	IdCompany INT NOT NULL,
	Ejercicio INT NOT NULL,
	IdCiclo INT NOT NULL,
	IdFase INT NOT NULL,
	IdCurrency INT NOT NULL,
	IdEpigrafe INT NOT NULL,
	Actuals DECIMAL(18,5) NOT NULL,
	PctActuals DECIMAL(18,5) NOT NULL,
	Budget DECIMAL(18,5) NOT NULL,
	PctBudget DECIMAL(18,5) NOT NULL,
	Variance DECIMAL(18,5) NOT NULL,
	Volume DECIMAL(18,5) NOT NULL,
	InventoryChange DECIMAL(18,5) NOT NULL,
	Mix DECIMAL(18,5) NOT NULL,
	`New` DECIMAL(18,5) NOT NULL,
	Economics DECIMAL(18,5) NOT NULL,
	QuickSavings DECIMAL(18,5) NOT NULL,
	CurrencyMix DECIMAL(18,5) NOT NULL,
	ExchangeRate DECIMAL(18,5) NOT NULL,
	RawMaterial DECIMAL(18,5) NOT NULL,
	Scrap DECIMAL(18,5) NOT NULL,
	IndustrialPerformance DECIMAL(18,5) NOT NULL,
	ProtoTooling DECIMAL(18,5) NOT NULL,
	Other DECIMAL(18,5) NOT NULL,
	`Check` DECIMAL(18,5) NOT NULL,
	Comments VARCHAR(4000) NULL,
	idCarga INT NULL,
	idCargaSTGBW INT NULL,
	idHoja INT NULL,
	PRIMARY KEY (Id)
);

CREATE TABLE IF NOT EXISTS DATA_BridgesMonth_BW (

	Id INT AUTO_INCREMENT NOT NULL,
	idAPICarga INT NOT NULL,
	guidCarga CHAR(36) NULL,
	FechaUltModif DATETIME NOT NULL,
	IdCompany INT NOT NULL,
	Ejercicio INT NOT NULL,
	IdCiclo INT NOT NULL,
	IdFase INT NOT NULL,
	IdCurrency INT NOT NULL,
	IdEpigrafe INT NOT NULL,
	Volume DECIMAL(18,5) NOT NULL,
	InventoryChange DECIMAL(18,5) NOT NULL,
	Mix DECIMAL(18,5) NOT NULL,
	`New` DECIMAL(18,5) NOT NULL,
	Economics DECIMAL(18,5) NOT NULL,
	QuickSavings DECIMAL(18,5) NOT NULL,
	CurrencyMix DECIMAL(18,5) NOT NULL,
	ExchangeRate DECIMAL(18,5) NOT NULL,
	RawMaterial DECIMAL(18,5) NOT NULL,
	Scrap DECIMAL(18,5) NOT NULL,
	IndustrialPerformance DECIMAL(18,5) NOT NULL,
	ProtoTooling DECIMAL(18,5) NOT NULL,
	Other DECIMAL(18,5) NOT NULL,
	`Check` DECIMAL(18,5) NOT NULL,
	Comments VARCHAR(4000) NULL,
	idCarga INT NULL,
	idCargaSTGBW INT NULL,
	idHoja INT NULL,
 	PRIMARY KEY (Id)
);

CREATE TABLE IF NOT EXISTS DATA_BridgesFY_BW (

	Id INT AUTO_INCREMENT NOT NULL,
	IdAPICarga INT NOT NULL,
	guidCarga CHAR(36) NULL,
	FechaUltModif DATETIME NOT NULL,
	IdCompany INT NOT NULL,
	Ejercicio INT NOT NULL,
	IdCiclo INT NOT NULL,
	IdFase INT NOT NULL,
	IdCurrency INT NOT NULL,
	IdEpigrafe INT NOT NULL,
	FiscalYear DECIMAL(18,5) NOT NULL,
	Percentage DECIMAL(18,5) NOT NULL,
	Zero DECIMAL(18,5) NOT NULL,
	ZeroPercentage DECIMAL(18,5) NOT NULL,
	Absolute DECIMAL(18,5) NOT NULL,
	AbsolutePercentage DECIMAL(18,5) NOT NULL,
	VMixNew DECIMAL(18,5) NOT NULL,
	RawMaterial DECIMAL(18,5) NOT NULL,
	Scrap DECIMAL(18,5) NOT NULL,
	Economics DECIMAL(18,5) NOT NULL,
	CurrencyMix DECIMAL(18,5) NOT NULL,
	Performance DECIMAL(18,5) NOT NULL,
	ProtoTool DECIMAL(18,5) NOT NULL,
	Others DECIMAL(18,5) NOT NULL,
	Comments VARCHAR(4000) NULL,
	idCarga INT NULL,
	idCargaSTGBW INT NULL,
	idHoja INT NULL,
 	PRIMARY KEY (Id)
);

CREATE TABLE IF NOT EXISTS DATA_Budget_BW (

	id INT AUTO_INCREMENT NOT NULL,
	idAPICarga INT NOT NULL,
	guidCarga CHAR(36) NOT NULL,
	FechaUltModif DATETIME NOT NULL,
	idCompany INT NOT NULL,
	ejercicio INT NOT NULL,
	idCiclo INT NOT NULL,
	idFase INT NOT NULL,
	idCurrency INT NOT NULL,
	idEpigrafe INT NOT NULL,
	mes00 DECIMAL(18,5) NOT NULL,
	mes01 DECIMAL(18,5) NOT NULL,
	mes02 DECIMAL(18,5) NOT NULL,
	mes03 DECIMAL(18,5) NOT NULL,
	mes04 DECIMAL(18,5) NOT NULL,
	mes05 DECIMAL(18,5) NOT NULL,
	mes06 DECIMAL(18,5) NOT NULL,
	mes07 DECIMAL(18,5) NOT NULL,
	mes08 DECIMAL(18,5) NOT NULL,
	mes09 DECIMAL(18,5) NOT NULL,
	mes10 DECIMAL(18,5) NOT NULL,
	mes11 DECIMAL(18,5) NOT NULL,
	mes12 DECIMAL(18,5) NOT NULL,
	mes13 DECIMAL(18,5) NOT NULL,
	idCarga INT NULL,
	idCargaSTGBW INT NULL,
	idHoja INT NULL,
PRIMARY KEY (id)
);

CREATE TABLE IF NOT EXISTS DATA_Forecast_BW (

	id INT AUTO_INCREMENT NOT NULL,
	idAPICarga INT NOT NULL,
	guidCarga CHAR(36) NOT NULL,
	FechaUltModif DATETIME NOT NULL,
	idCompany INT NOT NULL,
	ejercicio INT NOT NULL,
	idCiclo INT NOT NULL,
	idFase INT NOT NULL,
	idCurrency INT NOT NULL,
	idEpigrafe INT NOT NULL,
	mes00 DECIMAL(18,5) NOT NULL,
	mes01 DECIMAL(18,5) NOT NULL,
	mes02 DECIMAL(18,5) NOT NULL,
	mes03 DECIMAL(18,5) NOT NULL,
	mes04 DECIMAL(18,5) NOT NULL,
	mes05 DECIMAL(18,5) NOT NULL,
	mes06 DECIMAL(18,5) NOT NULL,
	mes07 DECIMAL(18,5) NOT NULL,
	mes08 DECIMAL(18,5) NOT NULL,
	mes09 DECIMAL(18,5) NOT NULL,
	mes10 DECIMAL(18,5) NOT NULL,
	mes11 DECIMAL(18,5) NOT NULL,
	mes12 DECIMAL(18,5) NOT NULL,
	mes13 DECIMAL(18,5) NOT NULL,
	idCarga INT NULL,
	idCargaSTGBW INT NULL,
	idHoja INT NULL,
PRIMARY KEY (id)
);

CREATE TABLE IF NOT EXISTS DATA_Actuals_BW (

	id INT AUTO_INCREMENT NOT NULL,
	idAPICarga INT NOT NULL,
	guidCarga CHAR(36) NOT NULL,
	FechaUltModif DATETIME NOT NULL,
	idCompany INT NOT NULL,
	ejercicio INT NOT NULL,
	idCiclo INT NOT NULL,
	idFase INT NOT NULL,
	idCurrency INT NOT NULL,
	idEpigrafe INT NOT NULL,
	mes00 DECIMAL(18,5) NOT NULL,
	mes01 DECIMAL(18,5) NOT NULL,
	mes02 DECIMAL(18,5) NOT NULL,
	mes03 DECIMAL(18,5) NOT NULL,
	mes04 DECIMAL(18,5) NOT NULL,
	mes05 DECIMAL(18,5) NOT NULL,
	mes06 DECIMAL(18,5) NOT NULL,
	mes07 DECIMAL(18,5) NOT NULL,
	mes08 DECIMAL(18,5) NOT NULL,
	mes09 DECIMAL(18,5) NOT NULL,
	mes10 DECIMAL(18,5) NOT NULL,
	mes11 DECIMAL(18,5) NOT NULL,
	mes12 DECIMAL(18,5) NOT NULL,
	mes13 DECIMAL(18,5) NOT NULL,
	idCarga INT NULL,
	idCargaSTGBW INT NULL,
	idHoja INT NULL,
PRIMARY KEY (id)
);

CREATE TABLE IF NOT EXISTS DATA_BridgesFY (

	Id INT AUTO_INCREMENT NOT NULL,
	IdAPICarga INT NOT NULL,
	guidCarga CHAR(36) NULL,
	FechaUltModif DATETIME NOT NULL,
	IdCompany INT NOT NULL,
	Ejercicio INT NOT NULL,
	IdCiclo INT NOT NULL,
	IdFase INT NOT NULL,
	IdCurrency INT NOT NULL,
	IdEpigrafe INT NOT NULL,
	FiscalYear DECIMAL(18,5) NOT NULL,
	Percentage DECIMAL(18,5) NOT NULL,
	Zero DECIMAL(18,5) NOT NULL,
	ZeroPercentage DECIMAL(18,5) NOT NULL,
	Absolute DECIMAL(18,5) NOT NULL,
	AbsolutePercentage DECIMAL(18,5) NOT NULL,
	VMixNew DECIMAL(18,5) NOT NULL,
	RawMaterial DECIMAL(18,5) NOT NULL,
	Scrap DECIMAL(18,5) NOT NULL,
	Economics DECIMAL(18,5) NOT NULL,
	CurrencyMix DECIMAL(18,5) NOT NULL,
	Performance DECIMAL(18,5) NOT NULL,
	ProtoTool DECIMAL(18,5) NOT NULL,
	Others DECIMAL(18,5) NOT NULL,
	Comments VARCHAR(4000) NULL,
 	PRIMARY KEY (Id)
);

CREATE TABLE IF NOT EXISTS LK_PLANT_TREE (

	idTree INT NOT NULL,
	idDivision INT NOT NULL,
	idDivisionCompany INT NOT NULL,
	idSubdivision INT NOT NULL,
	idCountry INT NOT NULL,
 PRIMARY KEY (idTree )
);

CREATE OR REPLACE VIEW V_LK_PLANT_TREE
AS
SELECT        slava_db.LK_PLANT_TREE.idTree, slava_db.LK_PLANT_TREE.idDivision, slava_db.LK_PLANT_DIVISION.Division, slava_db.LK_PLANT_TREE.idDivisionCompany, slava_db.LK_PLANT_DIVISION_COMPANY.DivisionCompany, 
                         slava_db.LK_PLANT_TREE.idSubdivision, slava_db.LK_PLANT_SUBDIVISION.Subdivision, slava_db.LK_PLANT_TREE.idCountry, slava_db.LK_PLANT_COUNTRY.Country
FROM            slava_db.LK_PLANT_DIVISION INNER JOIN
                         slava_db.LK_PLANT_TREE ON slava_db.LK_PLANT_DIVISION.idDivision = slava_db.LK_PLANT_TREE.idDivision INNER JOIN
                         slava_db.LK_PLANT_COUNTRY ON slava_db.LK_PLANT_TREE.idCountry = slava_db.LK_PLANT_COUNTRY.idCountry INNER JOIN
                         slava_db.LK_PLANT_SUBDIVISION ON slava_db.LK_PLANT_TREE.idSubdivision = slava_db.LK_PLANT_SUBDIVISION.idSubdivision INNER JOIN
                         slava_db.LK_PLANT_DIVISION_COMPANY ON slava_db.LK_PLANT_TREE.idDivisionCompany = slava_db.LK_PLANT_DIVISION_COMPANY.idDivisionCompany;


CREATE TABLE IF NOT EXISTS API_CARGAS_EXCEL (

	IdAPICarga INT AUTO_INCREMENT NOT NULL,
	guidCarga CHAR(36) NULL,
	FechaHoraInicio DATETIME NULL,
	FechaHoraFin DATETIME NULL,
	idCompany INT NOT NULL,
	idPlantilla INT NOT NULL,
	idVersionPlantilla INT NOT NULL,
	idBoton INT NOT NULL,
	idEstado INT NOT NULL,
	idCiclo INT NULL,
	idFase INT NULL,
	ejercicio INT NOT NULL,
PRIMARY KEY (IdAPICarga )
);

CREATE TABLE IF NOT EXISTS API_PASOS_EXCEL (

	IdAPIPaso INT AUTO_INCREMENT NOT NULL,
	IdAPICarga INT NOT NULL,
	FechaHoraInicio DATETIME NULL,
	FechaHoraFin DATETIME NULL,
	IdPaso INT NOT NULL,
	IdAPICall INT NOT NULL,
	IdEstado INT NOT NULL,
PRIMARY KEY (IdAPIPaso )
);

CREATE TABLE IF NOT EXISTS PLANTILLAS (

	idPlantilla INT NOT NULL,
	Plantilla VARCHAR(50) NOT NULL,
 PRIMARY KEY (idPlantilla )
);

CREATE TABLE IF NOT EXISTS log_actividad (

	id INT AUTO_INCREMENT NOT NULL,
	timestamp DATETIME NULL,
	nivel VARCHAR(20) NULL,
	modulo VARCHAR(100) NULL,
	entrada VARCHAR(2000) NULL,
	IdFicheroExcel INT NULL,
	NombreFicheroExcel VARCHAR(255) NULL,
	iDCarga INT NOT NULL,
	DCR_ID_JOB INT NOT NULL,
PRIMARY KEY (id)
);

CREATE TABLE IF NOT EXISTS DATA_BridgesMonth (

	Id INT AUTO_INCREMENT NOT NULL,
	idAPICarga INT NOT NULL,
	guidCarga CHAR(36) NULL,
	FechaUltModif DATETIME NOT NULL,
	IdCompany INT NOT NULL,
	Ejercicio INT NOT NULL,
	IdCiclo INT NOT NULL,
	IdFase INT NOT NULL,
	IdCurrency INT NOT NULL,
	IdEpigrafe INT NOT NULL,
	Actuals DECIMAL(18,5) NOT NULL,
	PctActuals DECIMAL(18,5) NOT NULL,
	Budget DECIMAL(18,5) NOT NULL,
	PctBudget DECIMAL(18,5) NOT NULL,
	Variance DECIMAL(18,5) NOT NULL,
	Volume DECIMAL(18,5) NOT NULL,
	InventoryChange DECIMAL(18,5) NOT NULL,
	Mix DECIMAL(18,5) NOT NULL,
	`New` DECIMAL(18,5) NOT NULL,
	EconomicsInflation DECIMAL(18,5) NOT NULL,
	EconomicsInflationClaim DECIMAL(18,5) NOT NULL,
	EconomicsPricingIssues DECIMAL(18,5) NOT NULL,
	EconomicsLTAS DECIMAL(18,5) NOT NULL,
	EconomicsBPs DECIMAL(18,5) NOT NULL,
	EconomicsComponentEffect DECIMAL(18,5) NOT NULL,
	StockValuation DECIMAL(18,5) NOT NULL,
	QuickSavings DECIMAL(18,5) NOT NULL,
	CurrencyMix DECIMAL(18,5) NOT NULL,
	ExchangeRate DECIMAL(18,5) NOT NULL,
	RawMaterial DECIMAL(18,5) NOT NULL,
	Scrap DECIMAL(18,5) NOT NULL,
	PerformanceGSD DECIMAL(18,5) NOT NULL,
	PerformanceQuality DECIMAL(18,5) NOT NULL,
	PerformanceOther DECIMAL(18,5) NOT NULL,
	PerformanceLaunchingCost DECIMAL(18,5) NOT NULL,
	ProtoTooling DECIMAL(18,5) NOT NULL,
	AccruralOthers DECIMAL(18,5) NOT NULL,
	OneTimeEffectOthers DECIMAL(18,5) NOT NULL,
	Other DECIMAL(18,5) NOT NULL,
	Comments VARCHAR(4000) NULL,
	PRIMARY KEY (Id)
);

CREATE TABLE IF NOT EXISTS PLANTILLAS_BOTONES_PASOS (

	id INT NOT NULL,
	idPaso INT NOT NULL,
	Paso VARCHAR(500) NOT NULL,
	idBoton INT NOT NULL,
	Activo BOOLEAN NOT NULL,
	Hoja VARCHAR(100) NULL,
	idPasoTipo INT NOT NULL,
	EsBotonSecundario BOOLEAN NOT NULL,
	descripcion VARCHAR(4000) NULL,
	orden INT NOT NULL,
	Origen VARCHAR(100) NULL,
	idInformeSAP INT NULL,
	NumReportsBW INT NULL,
	unidades VARCHAR(300) NULL,
	Rango VARCHAR(100) NULL,
	BloqueDatos VARCHAR(100) NULL,
	moneda VARCHAR(50) NULL,
 PRIMARY KEY (id)
);

CREATE TABLE IF NOT EXISTS API_APICall (

	IdAPICall INT AUTO_INCREMENT NOT NULL,
	IdAPICarga INT NOT NULL,
	CountOrigen INT NOT NULL,
	CountDestino INT NOT NULL,
	FechaHoraInicio DATETIME NOT NULL,
	FechaHoraFin DATETIME NOT NULL,
	CabecerasHTTP TEXT NULL,
	URL VARCHAR(1024) NULL,
	IPOrigen VARCHAR(1024) NULL,
	TipoLlamada INT NOT NULL,
PRIMARY KEY (IdAPICall )
);

CREATE TABLE IF NOT EXISTS API_LOG_ACTIVIDAD (

	Id INT AUTO_INCREMENT NOT NULL,
	MachineName VARCHAR(50) NULL,
	Logged DATETIME NULL,
	Level VARCHAR(50) NULL,
	Message TEXT NULL,
	Logger VARCHAR(250) NULL,
	Exception TEXT NULL,
	idAPICall INT NOT NULL,
	PRIMARY KEY (Id)
);

CREATE TABLE IF NOT EXISTS DATA_Tipo_Cambio (

	id INT AUTO_INCREMENT NOT NULL,
	idAPICarga INT NOT NULL,
	guidCarga CHAR(36) NOT NULL,
	FechaUltModif DATETIME NOT NULL,
	ejercicio INT NOT NULL,
	idCurrency INT NOT NULL,
	CalendarDay DATE NOT NULL,
	mes INT NOT NULL,
	P DECIMAL(18,5) NULL,
	FC DECIMAL(18,5) NULL,
	FB DECIMAL(18,5) NULL,
	idCarga INT NULL,
	idCargaSTGBW INT NULL,
	idHoja INT NULL,
PRIMARY KEY (id)
);

CREATE TABLE IF NOT EXISTS DATA_Actuals (

	id INT AUTO_INCREMENT NOT NULL,
	idAPICarga INT NOT NULL,
	guidCarga CHAR(36) NOT NULL,
	FechaUltModif DATETIME NOT NULL,
	idCompany INT NOT NULL,
	ejercicio INT NOT NULL,
	idCiclo INT NOT NULL,
	idFase INT NOT NULL,
	idCurrency INT NOT NULL,
	idEpigrafe INT NOT NULL,
	mes00 DECIMAL(18,5) NOT NULL,
	mes01 DECIMAL(18,5) NOT NULL,
	mes02 DECIMAL(18,5) NOT NULL,
	mes03 DECIMAL(18,5) NOT NULL,
	mes04 DECIMAL(18,5) NOT NULL,
	mes05 DECIMAL(18,5) NOT NULL,
	mes06 DECIMAL(18,5) NOT NULL,
	mes07 DECIMAL(18,5) NOT NULL,
	mes08 DECIMAL(18,5) NOT NULL,
	mes09 DECIMAL(18,5) NOT NULL,
	mes10 DECIMAL(18,5) NOT NULL,
	mes11 DECIMAL(18,5) NOT NULL,
	mes12 DECIMAL(18,5) NOT NULL,
	mes13 DECIMAL(18,5) NOT NULL,
PRIMARY KEY (id)
);

CREATE TABLE IF NOT EXISTS DATA_Budget (

	id INT AUTO_INCREMENT NOT NULL,
	idAPICarga INT NOT NULL,
	guidCarga CHAR(36) NOT NULL,
	FechaUltModif DATETIME NOT NULL,
	idCompany INT NOT NULL,
	ejercicio INT NOT NULL,
	idCiclo INT NOT NULL,
	idFase INT NOT NULL,
	idCurrency INT NOT NULL,
	idEpigrafe INT NOT NULL,
	mes00 DECIMAL(18,5) NOT NULL,
	mes01 DECIMAL(18,5) NOT NULL,
	mes02 DECIMAL(18,5) NOT NULL,
	mes03 DECIMAL(18,5) NOT NULL,
	mes04 DECIMAL(18,5) NOT NULL,
	mes05 DECIMAL(18,5) NOT NULL,
	mes06 DECIMAL(18,5) NOT NULL,
	mes07 DECIMAL(18,5) NOT NULL,
	mes08 DECIMAL(18,5) NOT NULL,
	mes09 DECIMAL(18,5) NOT NULL,
	mes10 DECIMAL(18,5) NOT NULL,
	mes11 DECIMAL(18,5) NOT NULL,
	mes12 DECIMAL(18,5) NOT NULL,
	mes13 DECIMAL(18,5) NOT NULL,
PRIMARY KEY (id)
);

CREATE TABLE IF NOT EXISTS EMAILS_AUT (

	idEmail INT NOT NULL,
	Mail_When VARCHAR(1000) NULL,
	Send_TO VARCHAR(1000) NULL,
	Send_CC VARCHAR(1000) NULL,
	Send_BCC VARCHAR(1000) NULL,
	Subject VARCHAR(500) NULL,
	Body VARCHAR(4000) NULL,
	LinkToSendAutEmail BOOLEAN NULL,
	Test VARCHAR(100) NULL,
 PRIMARY KEY (idEmail )
);

CREATE TABLE IF NOT EXISTS errorlog (

	ErrorLogID INT AUTO_INCREMENT NOT NULL,
	ErrorTime DATETIME NOT NULL,
	UserName VARCHAR(100) NOT NULL,
	ErrorNumber INT NOT NULL,
	ErrorSeverity INT NULL,
	ErrorState INT NULL,
	ErrorProcedure TEXT NULL,
	ErrorLine INT NULL,
	ErrorMessage TEXT NOT NULL,
	idAPICarga INT NULL,
 PRIMARY KEY (ErrorLogID )
);

CREATE TABLE IF NOT EXISTS LK_PLANT_CONTROLLERS (

	idCompanyController INT NOT NULL,
	IdCompany INT NOT NULL,
	Controller VARCHAR(200) NOT NULL,
	Email VARCHAR(200) NOT NULL,
 PRIMARY KEY (idCompanyController )
);

CREATE TABLE IF NOT EXISTS LK_PLANTILLAS_BOTONES_PASOS_TIPOS (

	idPasoTipo INT NOT NULL,
	Pasotipo VARCHAR(50) NOT NULL,
	descripcion VARCHAR(500) NULL,
 PRIMARY KEY (idPasoTipo )
);

CREATE TABLE IF NOT EXISTS PLANTILLAS_VERSIONES (

	idVersion INT NOT NULL,
	Version VARCHAR(50) NOT NULL,
	idPlantilla INT NOT NULL,
	Activa BOOLEAN NULL,
	Modificada DATETIME NULL,
	FechaActivacion DATETIME NULL,
	API_version VARCHAR(50) NULL,
	AdminWeb_version VARCHAR(50) NULL,
	Ejercicio INT NULL,
 PRIMARY KEY (idVersion )
);

CREATE TABLE IF NOT EXISTS STG_DATA_Actuals (

	id INT AUTO_INCREMENT NOT NULL,
	idAPICarga INT NOT NULL,
	guidCarga CHAR(36) NOT NULL,
	FechaUltModif DATETIME NOT NULL,
	idCompany INT NOT NULL,
	ejercicio INT NOT NULL,
	idCiclo INT NOT NULL,
	idFase INT NOT NULL,
	idCurrency INT NOT NULL,
	idEpigrafe INT NOT NULL,
	mes00 DECIMAL(18,5) NOT NULL,
	mes01 DECIMAL(18,5) NOT NULL,
	mes02 DECIMAL(18,5) NOT NULL,
	mes03 DECIMAL(18,5) NOT NULL,
	mes04 DECIMAL(18,5) NOT NULL,
	mes05 DECIMAL(18,5) NOT NULL,
	mes06 DECIMAL(18,5) NOT NULL,
	mes07 DECIMAL(18,5) NOT NULL,
	mes08 DECIMAL(18,5) NOT NULL,
	mes09 DECIMAL(18,5) NOT NULL,
	mes10 DECIMAL(18,5) NOT NULL,
	mes11 DECIMAL(18,5) NOT NULL,
	mes12 DECIMAL(18,5) NOT NULL,
	mes13 DECIMAL(18,5) NOT NULL,
PRIMARY KEY (id)
);

CREATE TABLE IF NOT EXISTS STG_DATA_BridgesFY (

	Id INT AUTO_INCREMENT NOT NULL,
	IdAPICarga INT NOT NULL,
	guidCarga CHAR(36) NULL,
	FechaUltModif DATETIME NOT NULL,
	IdCompany INT NOT NULL,
	Ejercicio INT NOT NULL,
	IdCiclo INT NOT NULL,
	IdFase INT NOT NULL,
	IdCurrency INT NOT NULL,
	IdEpigrafe INT NOT NULL,
	FiscalYear DECIMAL(18,5) NOT NULL,
	Percentage DECIMAL(18,5) NOT NULL,
	Zero DECIMAL(18,5) NOT NULL,
	ZeroPercentage DECIMAL(18,5) NOT NULL,
	Absolute DECIMAL(18,5) NOT NULL,
	AbsolutePercentage DECIMAL(18,5) NOT NULL,
	VMixNew DECIMAL(18,5) NOT NULL,
	RawMaterial DECIMAL(18,5) NOT NULL,
	Scrap DECIMAL(18,5) NOT NULL,
	Economics DECIMAL(18,5) NOT NULL,
	CurrencyMix DECIMAL(18,5) NOT NULL,
	Performance DECIMAL(18,5) NOT NULL,
	ProtoTool DECIMAL(18,5) NOT NULL,
	Others DECIMAL(18,5) NOT NULL,
	Comments VARCHAR(4000) NULL,
	PRIMARY KEY (Id)
);

CREATE TABLE IF NOT EXISTS STG_DATA_BridgesMonth (

	Id INT AUTO_INCREMENT NOT NULL,
	idAPICarga INT NOT NULL,
	guidCarga CHAR(36) NULL,
	FechaUltModif DATETIME NOT NULL,
	IdCompany INT NOT NULL,
	Ejercicio INT NOT NULL,
	IdCiclo INT NOT NULL,
	IdFase INT NOT NULL,
	IdCurrency INT NOT NULL,
	IdEpigrafe INT NOT NULL,
	Actuals DECIMAL(18,5) NOT NULL,
	PctActuals DECIMAL(18,5) NOT NULL,
	Budget DECIMAL(18,5) NOT NULL,
	PctBudget DECIMAL(18,5) NOT NULL,
	Variance DECIMAL(18,5) NOT NULL,
	Volume DECIMAL(18,5) NOT NULL,
	InventoryChange DECIMAL(18,5) NOT NULL,
	Mix DECIMAL(18,5) NOT NULL,
	`New` DECIMAL(18,5) NOT NULL,
	EconomicsInflation DECIMAL(18,5) NOT NULL,
	EconomicsInflationClaim DECIMAL(18,5) NOT NULL,
	EconomicsPricingIssues DECIMAL(18,5) NOT NULL,
	EconomicsLTAS DECIMAL(18,5) NOT NULL,
	EconomicsBPs DECIMAL(18,5) NOT NULL,
	EconomicsComponentEffect DECIMAL(18,5) NOT NULL,
	StockValuation DECIMAL(18,5) NOT NULL,
	QuickSavings DECIMAL(18,5) NOT NULL,
	CurrencyMix DECIMAL(18,5) NOT NULL,
	ExchangeRate DECIMAL(18,5) NOT NULL,
	RawMaterial DECIMAL(18,5) NOT NULL,
	Scrap DECIMAL(18,5) NOT NULL,
	PerformanceGSD DECIMAL(18,5) NOT NULL,
	PerformanceQuality DECIMAL(18,5) NOT NULL,
	PerformanceOther DECIMAL(18,5) NOT NULL,
	PerformanceLaunchingCost DECIMAL(18,5) NOT NULL,
	ProtoTooling DECIMAL(18,5) NOT NULL,
	AccruralOthers DECIMAL(18,5) NOT NULL,
	OneTimeEffectOthers DECIMAL(18,5) NOT NULL,
	Other DECIMAL(18,5) NOT NULL,
	Comments VARCHAR(4000) NULL,
	PRIMARY KEY (Id)
);

CREATE TABLE IF NOT EXISTS STG_DATA_Budget (

	id INT AUTO_INCREMENT NOT NULL,
	idAPICarga INT NOT NULL,
	guidCarga CHAR(36) NOT NULL,
	FechaUltModif DATETIME NOT NULL,
	idCompany INT NOT NULL,
	ejercicio INT NOT NULL,
	idCiclo INT NOT NULL,
	idFase INT NOT NULL,
	idCurrency INT NOT NULL,
	idEpigrafe INT NOT NULL,
	mes00 DECIMAL(18,5) NOT NULL,
	mes01 DECIMAL(18,5) NOT NULL,
	mes02 DECIMAL(18,5) NOT NULL,
	mes03 DECIMAL(18,5) NOT NULL,
	mes04 DECIMAL(18,5) NOT NULL,
	mes05 DECIMAL(18,5) NOT NULL,
	mes06 DECIMAL(18,5) NOT NULL,
	mes07 DECIMAL(18,5) NOT NULL,
	mes08 DECIMAL(18,5) NOT NULL,
	mes09 DECIMAL(18,5) NOT NULL,
	mes10 DECIMAL(18,5) NOT NULL,
	mes11 DECIMAL(18,5) NOT NULL,
	mes12 DECIMAL(18,5) NOT NULL,
	mes13 DECIMAL(18,5) NOT NULL,
PRIMARY KEY (id)
);

CREATE TABLE IF NOT EXISTS STG_DATA_Comentarios (

	Id INT AUTO_INCREMENT NOT NULL,
	IdAPICarga INT NOT NULL,
	guidCarga CHAR(36) NOT NULL,
	FechaUltModif DATETIME NULL,
	IdCompany INT NOT NULL,
	Ejercicio INT NOT NULL,
	IdCiclo INT NOT NULL,
	IdFase INT NOT NULL,
	IdEpigrafe INT NOT NULL,
	Etiqueta VARCHAR(50) NOT NULL,
	Comentario TEXT NULL,
 	PRIMARY KEY (Id)
);

CREATE TABLE IF NOT EXISTS STG_DATA_Forecast (

	id INT AUTO_INCREMENT NOT NULL,
	idAPICarga INT NOT NULL,
	guidCarga CHAR(36) NOT NULL,
	FechaUltModif DATETIME NULL,
	idCompany INT NOT NULL,
	ejercicio INT NOT NULL,
	idCiclo INT NOT NULL,
	idFase INT NOT NULL,
	idCurrency INT NOT NULL,
	idEpigrafe INT NOT NULL,
	mes00 DECIMAL(18,5) NOT NULL,
	mes01 DECIMAL(18,5) NOT NULL,
	mes02 DECIMAL(18,5) NOT NULL,
	mes03 DECIMAL(18,5) NOT NULL,
	mes04 DECIMAL(18,5) NOT NULL,
	mes05 DECIMAL(18,5) NOT NULL,
	mes06 DECIMAL(18,5) NOT NULL,
	mes07 DECIMAL(18,5) NOT NULL,
	mes08 DECIMAL(18,5) NOT NULL,
	mes09 DECIMAL(18,5) NOT NULL,
	mes10 DECIMAL(18,5) NOT NULL,
	mes11 DECIMAL(18,5) NOT NULL,
	mes12 DECIMAL(18,5) NOT NULL,
	mes13 DECIMAL(18,5) NOT NULL,
PRIMARY KEY (id)
);

CREATE TABLE IF NOT EXISTS UserLevelPermissions (

	UserLevelID INT NOT NULL,
	TableName VARCHAR(255) NOT NULL,
	Permission INT NOT NULL,
 PRIMARY KEY (UserLevelID,
	TableName )
);

CREATE TABLE IF NOT EXISTS UserLevels (

	UserLevelID INT NOT NULL,
	UserLevelName VARCHAR(255) NOT NULL,
 PRIMARY KEY (UserLevelID ) 

); 
ALTER TABLE API_CARGAS_EXCEL ALTER COLUMN idCiclo SET DEFAULT 0;

ALTER TABLE API_CARGAS_EXCEL ALTER COLUMN idFase SET DEFAULT 0;

ALTER TABLE API_CARGAS_EXCEL ALTER COLUMN ejercicio SET DEFAULT 0;

/*

ALTER TABLE CFG_CARGAS_EXCEL_COLUMNAS ALTER COLUMN esFiltro SET DEFAULT 0;

ALTER TABLE CFG_CARGAS_EXCEL_COLUMNAS ALTER COLUMN hacerCheckCabecera SET DEFAULT 0;

ALTER TABLE CFG_CARGAS_EXCEL_COLUMNAS ALTER COLUMN hacerRellenoCeldasVacias SET DEFAULT 0;

ALTER TABLE CFG_CARGAS_EXCEL_COLUMNAS.old ADD  DEFAULT 0 FOR esFiltro

ALTER TABLE CFG_CARGAS_EXCEL_COLUMNAS.old ADD  DEFAULT 0 FOR hacerCheckCabecera

ALTER TABLE CFG_CARGAS_EXCEL_COLUMNAS.old ADD  DEFAULT 0 FOR hacerRellenoCeldasVacias

*/

ALTER TABLE CONTROL 
MODIFY COLUMN SendAutEmail TINYINT(1) DEFAULT 0;

ALTER TABLE CONTROL 
MODIFY COLUMN BWReportsMandatory TINYINT(1) DEFAULT 1;

ALTER TABLE DATA_BridgesFY ALTER COLUMN FiscalYear SET DEFAULT 0;

ALTER TABLE DATA_BridgesFY ALTER COLUMN Percentage SET DEFAULT 0;

ALTER TABLE DATA_BridgesFY ALTER COLUMN Zero SET DEFAULT 0;

ALTER TABLE DATA_BridgesFY ALTER COLUMN ZeroPercentage SET DEFAULT 0;

ALTER TABLE DATA_BridgesFY ALTER COLUMN Absolute SET DEFAULT 0;

ALTER TABLE DATA_BridgesFY ALTER COLUMN AbsolutePercentage SET DEFAULT 0;

ALTER TABLE DATA_BridgesFY ALTER COLUMN VMixNew SET DEFAULT 0;

ALTER TABLE DATA_BridgesFY ALTER COLUMN RawMaterial SET DEFAULT 0;

ALTER TABLE DATA_BridgesFY ALTER COLUMN Scrap SET DEFAULT 0;

ALTER TABLE DATA_BridgesFY ALTER COLUMN Economics SET DEFAULT 0;

ALTER TABLE DATA_BridgesFY ALTER COLUMN CurrencyMix SET DEFAULT 0;

ALTER TABLE DATA_BridgesFY ALTER COLUMN Performance SET DEFAULT 0;

ALTER TABLE DATA_BridgesFY ALTER COLUMN ProtoTool SET DEFAULT 0;

ALTER TABLE DATA_BridgesFY ALTER COLUMN Others SET DEFAULT 0;

ALTER TABLE DATA_BridgesFY ALTER COLUMN Comments SET DEFAULT '';

ALTER TABLE DATA_BridgesFY_BW ALTER COLUMN FiscalYear SET DEFAULT 0;

ALTER TABLE DATA_BridgesFY_BW ALTER COLUMN Percentage SET DEFAULT 0;

ALTER TABLE DATA_BridgesFY_BW ALTER COLUMN Zero SET DEFAULT 0;

ALTER TABLE DATA_BridgesFY_BW ALTER COLUMN ZeroPercentage SET DEFAULT 0;

ALTER TABLE DATA_BridgesFY_BW ALTER COLUMN Absolute SET DEFAULT 0;

ALTER TABLE DATA_BridgesFY_BW ALTER COLUMN AbsolutePercentage SET DEFAULT 0;

ALTER TABLE DATA_BridgesFY_BW ALTER COLUMN VMixNew SET DEFAULT 0;

ALTER TABLE DATA_BridgesFY_BW ALTER COLUMN RawMaterial SET DEFAULT 0;

ALTER TABLE DATA_BridgesFY_BW ALTER COLUMN Scrap SET DEFAULT 0;

ALTER TABLE DATA_BridgesFY_BW ALTER COLUMN Economics SET DEFAULT 0;

ALTER TABLE DATA_BridgesFY_BW ALTER COLUMN CurrencyMix SET DEFAULT 0;

ALTER TABLE DATA_BridgesFY_BW ALTER COLUMN Performance SET DEFAULT 0;

ALTER TABLE DATA_BridgesFY_BW ALTER COLUMN ProtoTool SET DEFAULT 0;

ALTER TABLE DATA_BridgesFY_BW ALTER COLUMN Others SET DEFAULT 0;

ALTER TABLE DATA_BridgesFY_BW ALTER COLUMN Comments SET DEFAULT '';

ALTER TABLE DATA_BRIDGESFY_BW_EUR ALTER COLUMN Actuals SET DEFAULT 0;

ALTER TABLE DATA_BRIDGESFY_BW_EUR ALTER COLUMN PctActuals SET DEFAULT 0;

ALTER TABLE DATA_BRIDGESFY_BW_EUR ALTER COLUMN Budget SET DEFAULT 0;

ALTER TABLE DATA_BRIDGESFY_BW_EUR ALTER COLUMN PctBudget SET DEFAULT 0;

ALTER TABLE DATA_BRIDGESFY_BW_EUR ALTER COLUMN Variance SET DEFAULT 0;

ALTER TABLE DATA_BRIDGESFY_BW_EUR ALTER COLUMN Volume SET DEFAULT 0;

ALTER TABLE DATA_BRIDGESFY_BW_EUR ALTER COLUMN InventoryChange SET DEFAULT 0;

ALTER TABLE DATA_BRIDGESFY_BW_EUR ALTER COLUMN Mix SET DEFAULT 0;

ALTER TABLE DATA_BRIDGESFY_BW_EUR ALTER COLUMN `New` SET DEFAULT 0;

ALTER TABLE DATA_BRIDGESFY_BW_EUR ALTER COLUMN Economics SET DEFAULT 0;

ALTER TABLE DATA_BRIDGESFY_BW_EUR ALTER COLUMN QuickSavings SET DEFAULT 0;

ALTER TABLE DATA_BRIDGESFY_BW_EUR ALTER COLUMN CurrencyMix SET DEFAULT 0;

ALTER TABLE DATA_BRIDGESFY_BW_EUR ALTER COLUMN ExchangeRate SET DEFAULT 0;

ALTER TABLE DATA_BRIDGESFY_BW_EUR ALTER COLUMN RawMaterial SET DEFAULT 0;

ALTER TABLE DATA_BRIDGESFY_BW_EUR ALTER COLUMN Scrap SET DEFAULT 0;

ALTER TABLE DATA_BRIDGESFY_BW_EUR ALTER COLUMN IndustrialPerformance SET DEFAULT 0;

ALTER TABLE DATA_BRIDGESFY_BW_EUR ALTER COLUMN ProtoTooling SET DEFAULT 0;

ALTER TABLE DATA_BRIDGESFY_BW_EUR ALTER COLUMN Other SET DEFAULT 0;

ALTER TABLE DATA_BRIDGESFY_BW_EUR ALTER COLUMN `Check` SET DEFAULT 0;

ALTER TABLE DATA_BridgesMonth_BW ALTER COLUMN Volume SET DEFAULT 0;

ALTER TABLE DATA_BridgesMonth_BW ALTER COLUMN InventoryChange SET DEFAULT 0;

ALTER TABLE DATA_BridgesMonth_BW ALTER COLUMN Mix SET DEFAULT 0;

ALTER TABLE DATA_BridgesMonth_BW ALTER COLUMN `New` SET DEFAULT 0;

ALTER TABLE DATA_BridgesMonth_BW ALTER COLUMN Economics SET DEFAULT 0;

ALTER TABLE DATA_BridgesMonth_BW ALTER COLUMN QuickSavings SET DEFAULT 0;

ALTER TABLE DATA_BridgesMonth_BW ALTER COLUMN CurrencyMix SET DEFAULT 0;

ALTER TABLE DATA_BridgesMonth_BW ALTER COLUMN ExchangeRate SET DEFAULT 0;

ALTER TABLE DATA_BridgesMonth_BW ALTER COLUMN RawMaterial SET DEFAULT 0;

ALTER TABLE DATA_BridgesMonth_BW ALTER COLUMN Scrap SET DEFAULT 0;

ALTER TABLE DATA_BridgesMonth_BW ALTER COLUMN IndustrialPerformance SET DEFAULT 0;

ALTER TABLE DATA_BridgesMonth_BW ALTER COLUMN ProtoTooling SET DEFAULT 0;

ALTER TABLE DATA_BridgesMonth_BW ALTER COLUMN Other SET DEFAULT 0;

ALTER TABLE DATA_BridgesMonth_BW ALTER COLUMN `Check` SET DEFAULT 0;

/*

ALTER TABLE DATA_BridgesMonth_BW_EUR ALTER COLUMN Actuals SET DEFAULT 0;

ALTER TABLE DATA_BridgesMonth_BW_EUR ALTER COLUMN PctActuals SET DEFAULT 0;

ALTER TABLE DATA_BridgesMonth_BW_EUR ALTER COLUMN Budget SET DEFAULT 0;

ALTER TABLE DATA_BridgesMonth_BW_EUR ALTER COLUMN PctBudget SET DEFAULT 0;

ALTER TABLE DATA_BridgesMonth_BW_EUR ALTER COLUMN Variance SET DEFAULT 0;

ALTER TABLE DATA_BridgesMonth_BW_EUR ALTER COLUMN Volume SET DEFAULT 0;

ALTER TABLE DATA_BridgesMonth_BW_EUR ALTER COLUMN InventoryChange SET DEFAULT 0;

ALTER TABLE DATA_BridgesMonth_BW_EUR ALTER COLUMN Mix SET DEFAULT 0;

ALTER TABLE DATA_BridgesMonth_BW_EUR ALTER COLUMN `New` SET DEFAULT 0;

ALTER TABLE DATA_BridgesMonth_BW_EUR ALTER COLUMN Economics SET DEFAULT 0;

ALTER TABLE DATA_BridgesMonth_BW_EUR ALTER COLUMN QuickSavings SET DEFAULT 0;

ALTER TABLE DATA_BridgesMonth_BW_EUR ALTER COLUMN CurrencyMix SET DEFAULT 0;

ALTER TABLE DATA_BridgesMonth_BW_EUR ALTER COLUMN ExchangeRate SET DEFAULT 0;

ALTER TABLE DATA_BridgesMonth_BW_EUR ALTER COLUMN RawMaterial SET DEFAULT 0;

ALTER TABLE DATA_BridgesMonth_BW_EUR ALTER COLUMN Scrap SET DEFAULT 0;

ALTER TABLE DATA_BridgesMonth_BW_EUR ALTER COLUMN IndustrialPerformance SET DEFAULT 0;

ALTER TABLE DATA_BridgesMonth_BW_EUR ALTER COLUMN ProtoTooling SET DEFAULT 0;

ALTER TABLE DATA_BridgesMonth_BW_EUR ALTER COLUMN Other SET DEFAULT 0;

ALTER TABLE DATA_BridgesMonth_BW_EUR ALTER COLUMN `Check` SET DEFAULT 0;

*/

-- ALTER TABLE DATA_Tipo_Cambio ALTER COLUMN FechaUltModif SET DEFAULT NULL;

ALTER TABLE DATA_Tipo_Cambio ALTER COLUMN P SET DEFAULT 0;

ALTER TABLE DATA_Tipo_Cambio ALTER COLUMN FC SET DEFAULT 0;

ALTER TABLE DATA_Tipo_Cambio ALTER COLUMN FB SET DEFAULT 0;

/*

ALTER TABLE JOBS_CARGAS_STG_BW_LOG
MODIFY COLUMN `timestamp` DATETIME DEFAULT CURRENT_TIMESTAMP;

ALTER TABLE JOBS_CARGAS_STG_BW_LOG ALTER COLUMN iDCargaSTGBW SET DEFAULT 1;

*/

ALTER TABLE log_actividad
MODIFY COLUMN `timestamp` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP;

ALTER TABLE log_actividad ALTER COLUMN iDCarga SET DEFAULT 0;

ALTER TABLE log_actividad ALTER COLUMN DCR_ID_JOB SET DEFAULT 0;

/*

ALTER TABLE PLANTILLAS_ESTADOSUPD ALTER COLUMN idCompany SET DEFAULT 0;

ALTER TABLE PLANTILLAS_ESTADOSUPD ALTER COLUMN Ejercicio SET DEFAULT 0;

ALTER TABLE PLANTILLAS_ESTADOSUPD ALTER COLUMN idCiclo SET DEFAULT 0;

ALTER TABLE PLANTILLAS_ESTADOSUPD ALTER COLUMN idFase SET DEFAULT 0;

ALTER TABLE PLANTILLAS_ESTADOSUPD ALTER COLUMN idPlantilla SET DEFAULT 0;

ALTER TABLE REGVAL_LOG ADD  CONSTRAINT DF_REGVAL_LOG_FechaLog  DEFAULT (NOW()) FOR FechaLog;

*/



CREATE PROCEDURE sp_Upsert_DATA_Actuals(IN p_guidCarga CHAR(36))
BEGIN
    DECLARE v_idAPICarga INT;
    DECLARE v_idCompany INT;
    DECLARE v_ejercicio INT;
    DECLARE v_idCiclo INT;
    DECLARE v_idFase INT;
    DECLARE v_DeletedCount INT DEFAULT 0;
    DECLARE v_InsertedCount INT DEFAULT 0;
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        -- Insertar error en tabla ErrorLog
        INSERT INTO ErrorLog (
            ErrorTime, UserName, ErrorNumber, ErrorSeverity, ErrorState,
            ErrorProcedure, ErrorLine, ErrorMessage, idAPICarga
        )
        VALUES (
            CURRENT_TIMESTAMP,
            CURRENT_USER(),
            NULL, -- MySQL no tiene ERROR_NUMBER(), usar NULL o personalizar
            NULL, -- MySQL no tiene ERROR_SEVERITY()
            NULL, -- MySQL no tiene ERROR_STATE()
            'sp_Upsert_DATA_Actuals',
            NULL, -- línea
            'Error en procedimiento',
            v_idAPICarga
        );
        -- Re-lanzar el error
        RESIGNAL;
    END;

    -- Selección de variables desde staging
    SELECT 
        idAPICarga, idCompany, ejercicio, idCiclo, idFase
    INTO 
        v_idAPICarga, v_idCompany, v_ejercicio, v_idCiclo, v_idFase
    FROM STG_DATA_Actuals
    WHERE guidCarga = p_guidCarga
    LIMIT 1;

    -- Borrar datos existentes en tabla final
    DELETE FROM DATA_Actuals
    WHERE idCompany = v_idCompany
      AND ejercicio = v_ejercicio
      AND idCiclo = v_idCiclo
      AND idFase = v_idFase
      AND idEpigrafe IN (
          SELECT idEpigrafe
          FROM STG_DATA_Actuals
          WHERE guidCarga = p_guidCarga
          GROUP BY idEpigrafe
      );

    SET v_DeletedCount = ROW_COUNT();

    -- Insertar datos desde staging
    INSERT INTO DATA_Actuals (
        idAPICarga, guidCarga, FechaUltModif,
        idCompany, ejercicio, idCiclo, idFase,
        idCurrency, idEpigrafe,
        mes00, mes01, mes02, mes03, mes04, mes05, mes06,
        mes07, mes08, mes09, mes10, mes11, mes12, mes13
    )
    SELECT 
        v_idAPICarga, p_guidCarga, CURRENT_TIMESTAMP,
        idCompany, ejercicio, idCiclo, idFase,
        idCurrency, idEpigrafe,
        mes00, mes01, mes02, mes03, mes04, mes05, mes06,
        mes07, mes08, mes09, mes10, mes11, mes12, mes13
    FROM STG_DATA_Actuals
    WHERE guidCarga = p_guidCarga
      AND idCompany = v_idCompany
      AND ejercicio = v_ejercicio
      AND idCiclo = v_idCiclo
      AND idFase = v_idFase;

    SET v_InsertedCount = ROW_COUNT();

    -- Borrar datos de staging
    DELETE FROM STG_DATA_Actuals
    WHERE guidCarga = p_guidCarga;

    -- Mostrar resultado
    SELECT CONCAT('Número de filas insertadas: ', v_InsertedCount) AS Mensaje;
    SELECT CONCAT('Número de filas actualizadas: ', v_DeletedCount) AS Mensaje;

END$$



CREATE PROCEDURE sp_Upsert_DATA_BridgesFY(IN p_guidCarga CHAR(36))
BEGIN
    DECLARE v_idAPICarga INT;
    DECLARE v_idCompany INT;
    DECLARE v_ejercicio INT;
    DECLARE v_idCiclo INT;
    DECLARE v_idFase INT;
    DECLARE v_DeletedCount INT DEFAULT 0;
    DECLARE v_InsertedCount INT DEFAULT 0;

    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        -- Insertar error en tabla ErrorLog
        INSERT INTO ErrorLog (
            ErrorTime, UserName, ErrorNumber, ErrorSeverity, ErrorState,
            ErrorProcedure, ErrorLine, ErrorMessage, idAPICarga
        )
        VALUES (
            CURRENT_TIMESTAMP,
            CURRENT_USER(),
            NULL, -- No hay ERROR_NUMBER() en MySQL
            NULL, -- No hay ERROR_SEVERITY()
            NULL, -- No hay ERROR_STATE()
            'sp_Upsert_DATA_BridgesFY',
            NULL,
            'Error en procedimiento',
            v_idAPICarga
        );
        RESIGNAL;
    END;

    -- Selección de variables desde staging
    SELECT idAPICarga, idCompany, ejercicio, idCiclo, idFase
    INTO v_idAPICarga, v_idCompany, v_ejercicio, v_idCiclo, v_idFase
    FROM STG_DATA_BridgesFY
    WHERE guidCarga = p_guidCarga
    LIMIT 1;

    -- Borrar datos existentes en tabla final
    DELETE FROM DATA_BridgesFY
    WHERE idCompany = v_idCompany
      AND ejercicio = v_ejercicio
      AND idCiclo = v_idCiclo
      AND idFase = v_idFase
      AND idEpigrafe IN (
          SELECT idEpigrafe
          FROM STG_DATA_BridgesFY
          WHERE guidCarga = p_guidCarga
          GROUP BY idEpigrafe
      );

    SET v_DeletedCount = ROW_COUNT();

    -- Insertar datos desde staging
    INSERT INTO DATA_BridgesFY (
        idAPICarga, guidCarga, FechaUltModif,
        idCompany, ejercicio, idCiclo, idFase,
        idCurrency, idEpigrafe,
        FiscalYear, `Percentage`, `Zero`, ZeroPercentage, `Absolute`,
        AbsolutePercentage, VMixNew, RawMaterial, Scrap, Economics,
        CurrencyMix, Performance, ProtoTool, Others, Comments
    )
    SELECT 
        v_idAPICarga, p_guidCarga, CURRENT_TIMESTAMP,
        idCompany, ejercicio, idCiclo, idFase,
        idCurrency, idEpigrafe,
        FiscalYear, `Percentage`, `Zero`, ZeroPercentage, `Absolute`,
        AbsolutePercentage, VMixNew, RawMaterial, Scrap, Economics,
        CurrencyMix, Performance, ProtoTool, Others, Comments
    FROM STG_DATA_BridgesFY
    WHERE guidCarga = p_guidCarga
      AND idCompany = v_idCompany
      AND ejercicio = v_ejercicio
      AND idCiclo = v_idCiclo
      AND idFase = v_idFase;

    SET v_InsertedCount = ROW_COUNT();

    -- Borrar datos de staging
    DELETE FROM STG_DATA_BridgesFY
    WHERE guidCarga = p_guidCarga;

    -- Mostrar resultado
    SELECT CONCAT('Número de filas insertadas: ', v_InsertedCount) AS Mensaje;
    SELECT CONCAT('Número de filas actualizadas: ', v_DeletedCount) AS Mensaje;

END$$



CREATE PROCEDURE sp_Upsert_DATA_BridgesMonth (IN in_guidCarga CHAR(36), IN in_idAPICarga INT, IN in_idCompany INT, IN in_ejercicio INT, IN in_idCiclo INT, IN in_idFase INT, IN in_etiqueta VARCHAR(20), IN in_DeletedCount INT, IN in_InsertedCount INT, IN in_idCompany VARCHAR(255), IN in_ejercicio VARCHAR(255), IN in_idCiclo VARCHAR(255), IN in_idFase VARCHAR(255), IN in_guidCarga VARCHAR(255), IN in_ROWCOUNT VARCHAR(255), IN in_guidCarga VARCHAR(255), IN in_idCompany VARCHAR(255), IN in_ejercicio VARCHAR(255), IN in_idCiclo VARCHAR(255), IN in_idFase VARCHAR(255), IN in_guidCarga INT, IN in_InsertedCount VARCHAR(255), IN in_DeletedCount VARCHAR(255), IN in_idAPICarga VARCHAR(255)) 
BEGIN

    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        DECLARE v_sqlstate CHAR(5);
        DECLARE v_errno INT;
        DECLARE v_message TEXT;
        GET DIAGNOSTICS CONDITION 1 v_sqlstate = RETURNED_SQLSTATE, v_errno = MYSQL_ERRNO, v_message = MESSAGE_TEXT;
        INSERT INTO errorlog (ErrorTime, UserName, ErrorNumber, ErrorSeverity, ErrorState, ErrorProcedure, ErrorLine, ErrorMessage, idAPICarga) 
        VALUES (NOW(), CURRENT_USER(), v_errno, NULL, NULL, 'sp_Upsert_DATA_BridgesMonth', NULL, v_message, IFNULL(idAPICarga, NULL)
);
        RESIGNAL;
    END;
    
in_guidCarga UNIQUEIDENTIFIER
AS
BEGIN
    

	

-- borramos datos existentes para idCompany, in_ejercicio,in_idCiclo,in_idFase,in_etiqueta
		-- SOLO permite una combinacion
		DECLARE idAPICarga INT DEFAULT NULL;
		DECLARE idCompany INT DEFAULT NULL;
		DECLARE ejercicio INT DEFAULT NULL;
		DECLARE idCiclo INT DEFAULT NULL;
		DECLARE idFase INT DEFAULT NULL;
		--DECLARE in_etiqueta VARCHAR(20) 

		DECLARE DeletedCount INT DEFAULT NULL;
		DECLARE InsertedCount INT DEFAULT NULL;

		-- seleccion de datos 
		SELECT
			in_idAPICarga		= idAPICarga
			, in_idCompany	= idCompany
			, in_ejercicio	= ejercicio
			, in_idCiclo		= idCiclo
			, in_idFase		= idFase
		FROM STG_DATA_BridgesMonth
		WHERE
			guidCarga = in_guidCarga
 LIMIT 1;
		-- borra todos los datos de la combinacion en la tabla final
		DELETE FROM DATA_BridgesMonth
		WHERE 
			idCompany		= in_idCompany	
			AND ejercicio	= in_ejercicio
			AND idCiclo	= in_idCiclo	
			AND idFase	= in_idFase	
			AND idEpigrafe IN (
				SELECT idepigrafe 
				FROM STG_DATA_BridgesMonth 
				WHERE guidCarga = in_guidCarga 
				group by idEpigrafe
				) 
		SELECT in_DeletedCount = @in_ROWCOUNT 

		INSERT INTO DATA_BridgesMonth (
			idAPICarga, guidCarga, FechaUltModif, 
			idCompany, ejercicio, idCiclo, idFase, 
			idCurrency, idEpigrafe,
            Actuals, PctActuals, Budget, PctBudget, Variance,
			Volume, InventoryChange, Mix, `New`, 
			EconomicsInflation, 
			EconomicsInflationClaim, EconomicsPricingIssues, EconomicsLTAS, EconomicsBPs, EconomicsComponentEffect,
			StockValuation, QuickSavings, CurrencyMix, ExchangeRate, RawMaterial,
			Scrap, PerformanceGSD, PerformanceQuality, PerformanceOther, PerformanceLaunchingCost,
			ProtoTooling, AccruralOthers, OneTimeEffectOthers, Other, Comments
		) 
		SELECT 
			in_idAPICarga, in_guidCarga, NOW(), 
			in_idCompany, in_ejercicio, in_idCiclo, in_idFase, 
			idCurrency, idEpigrafe,
            Actuals, PctActuals, Budget, PctBudget, Variance,
			Volume, InventoryChange, Mix, `New`, 
			EconomicsInflation, 
			EconomicsInflationClaim, EconomicsPricingIssues, EconomicsLTAS, EconomicsBPs, EconomicsComponentEffect,
			StockValuation, QuickSavings, CurrencyMix, ExchangeRate, RawMaterial,
			Scrap, PerformanceGSD, PerformanceQuality, PerformanceOther, PerformanceLaunchingCost,
			ProtoTooling, AccruralOthers, OneTimeEffectOthers, Other, Comments
		FROM 
			STG_DATA_BridgesMonth
		WHERE 
			guidCarga = in_guidCarga
			AND idCompany	= in_idCompany	
			AND ejercicio	= in_ejercicio
			AND idCiclo	= in_idCiclo	
			AND idFase	= in_idFase	
		
		SELECT in_InsertedCount = @in_ROWCOUNT 

		-- borrado de datos de staging
		DELETE 
		FROM STG_DATA_BridgesMonth
		WHERE guidCarga = in_guidCarga

		SELECT 'Número de filas insertadas: ' AS _msg;+ CAST(in_InsertedCount AS NVARCHAR(10)) 
		SELECT 'Número de filas actualizadas: ' AS _msg;+ CAST(in_DeletedCount AS NVARCHAR(10)) 

	
    
	BEGIN CATCH
        -- Insertar el error en la tabla de registro de errores
        INSERT INTO ErrorLog (
            ErrorTime, 
            UserName, 
            ErrorNumber, 
            ErrorSeverity, 
            ErrorState, 
            ErrorProcedure, 
            ErrorLine, 
            ErrorMessage,
			idAPICarga
        ) 
        VALUES (
            NOW(),
            CURRENT_USER(),
            ERROR_NUMBER(),
            ERROR_SEVERITY(),
            ERROR_STATE(),
            ERROR_PROCEDURE(),
            ERROR_LINE(),
            ERROR_MESSAGE(),
			in_idAPICarga
        ) 

        -- Re-lanzar el error para que sea visible al llamador
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Unhandled error'

END$$



CREATE PROCEDURE sp_Upsert_DATA_Budget (IN in_guidCarga CHAR(36), IN in_idAPICarga INT, IN in_idCompany INT, IN in_ejercicio INT, IN in_idCiclo INT, IN in_idFase INT, IN in_etiqueta VARCHAR(20), IN in_DeletedCount INT, IN in_InsertedCount INT, IN in_idCompany VARCHAR(255), IN in_ejercicio VARCHAR(255), IN in_idCiclo VARCHAR(255), IN in_idFase VARCHAR(255), IN in_guidCarga VARCHAR(255), IN in_ROWCOUNT VARCHAR(255), IN in_guidCarga VARCHAR(255), IN in_idCompany VARCHAR(255), IN in_ejercicio VARCHAR(255), IN in_idCiclo VARCHAR(255), IN in_idFase VARCHAR(255), IN in_guidCarga INT, IN in_InsertedCount VARCHAR(255), IN in_DeletedCount VARCHAR(255), IN in_idAPICarga VARCHAR(255)) 
BEGIN

    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        DECLARE v_sqlstate CHAR(5);
        DECLARE v_errno INT;
        DECLARE v_message TEXT;
        GET DIAGNOSTICS CONDITION 1 v_sqlstate = RETURNED_SQLSTATE, v_errno = MYSQL_ERRNO, v_message = MESSAGE_TEXT;
        INSERT INTO errorlog (ErrorTime, UserName, ErrorNumber, ErrorSeverity, ErrorState, ErrorProcedure, ErrorLine, ErrorMessage, idAPICarga) 
        VALUES (NOW(), CURRENT_USER(), v_errno, NULL, NULL, 'sp_Upsert_DATA_Budget', NULL, v_message, IFNULL(idAPICarga, NULL)
);
        RESIGNAL;
    END;
    
in_guidCarga UNIQUEIDENTIFIER
AS
BEGIN
    

	

-- borramos datos existentes para idCompany, in_ejercicio,in_idCiclo,in_idFase,in_etiqueta
		-- SOLO permite una combinacion
		DECLARE idAPICarga INT DEFAULT NULL;
		DECLARE idCompany INT DEFAULT NULL;
		DECLARE ejercicio INT DEFAULT NULL;
		DECLARE idCiclo INT DEFAULT NULL;
		DECLARE idFase INT DEFAULT NULL;
		--DECLARE in_etiqueta VARCHAR(20) 

		DECLARE DeletedCount INT DEFAULT NULL;
		DECLARE InsertedCount INT DEFAULT NULL;

		-- seleccion de datos 
		SELECT
			in_idAPICarga		= idAPICarga
			, in_idCompany	= idCompany
			, in_ejercicio	= ejercicio
			, in_idCiclo		= idCiclo
			, in_idFase		= idFase
		FROM STG_DATA_Budget
		WHERE
			guidCarga = in_guidCarga
 LIMIT 1;
		-- borra todos los datos de la combinacion en la tabla final
		DELETE FROM DATA_Budget
		WHERE 
			idCompany		= in_idCompany	
			AND ejercicio	= in_ejercicio
			AND idCiclo	= in_idCiclo	
			AND idFase	= in_idFase	
			AND idEpigrafe IN (
				SELECT idepigrafe 
				FROM STG_DATA_Budget 
				WHERE guidCarga = in_guidCarga 
				group by idEpigrafe
				) 
		SELECT in_DeletedCount = @in_ROWCOUNT 

		INSERT INTO DATA_Budget (
			idAPICarga, guidCarga, FechaUltModif, 
			idCompany, ejercicio, idCiclo, idFase, 
			idCurrency, idEpigrafe,
			mes00, mes01, mes02, mes03, mes04, mes05, mes06,
			mes07, mes08, mes09, mes10, mes11, mes12, mes13
		) 
		SELECT 
			in_idAPICarga, in_guidCarga, NOW(), 
			in_idCompany, in_ejercicio, in_idCiclo, in_idFase, 
			idCurrency, idEpigrafe,
			mes00, mes01, mes02, mes03, mes04, mes05, mes06,
			mes07, mes08, mes09, mes10, mes11, mes12, mes13
		FROM 
			STG_DATA_Budget
		WHERE 
			guidCarga = in_guidCarga
			AND idCompany	= in_idCompany	
			AND ejercicio	= in_ejercicio
			AND idCiclo	= in_idCiclo	
			AND idFase	= in_idFase	
		
		SELECT in_InsertedCount = @in_ROWCOUNT 

		-- borrado de datos de staging
		DELETE 
		FROM STG_DATA_Budget
		WHERE guidCarga = in_guidCarga

		SELECT 'Número de filas insertadas: ' AS _msg;+ CAST(in_InsertedCount AS NVARCHAR(10)) 
		SELECT 'Número de filas actualizadas: ' AS _msg;+ CAST(in_DeletedCount AS NVARCHAR(10)) 

	
    
	BEGIN CATCH
        -- Insertar el error en la tabla de registro de errores
        INSERT INTO ErrorLog (
            ErrorTime, 
            UserName, 
            ErrorNumber, 
            ErrorSeverity, 
            ErrorState, 
            ErrorProcedure, 
            ErrorLine, 
            ErrorMessage,
			idAPICarga
        ) 
        VALUES (
            NOW(),
            CURRENT_USER(),
            ERROR_NUMBER(),
            ERROR_SEVERITY(),
            ERROR_STATE(),
            ERROR_PROCEDURE(),
            ERROR_LINE(),
            ERROR_MESSAGE(),
			in_idAPICarga
        ) 

        -- Re-lanzar el error para que sea visible al llamador
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Unhandled error'

END$$



CREATE PROCEDURE sp_Upsert_DATA_Comentarios (IN in_guidCarga CHAR(36), IN in_guidCarga VARCHAR(255), IN in_etiqueta NVARCHAR(100), IN in_guidCarga VARCHAR(255), IN in_etiqueta VARCHAR(255), IN in_etiqueta VARCHAR(255), IN in_guidCarga VARCHAR(255)) 
BEGIN

    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        DECLARE v_sqlstate CHAR(5);
        DECLARE v_errno INT;
        DECLARE v_message TEXT;
        GET DIAGNOSTICS CONDITION 1 v_sqlstate = RETURNED_SQLSTATE, v_errno = MYSQL_ERRNO, v_message = MESSAGE_TEXT;
        INSERT INTO errorlog (ErrorTime, UserName, ErrorNumber, ErrorSeverity, ErrorState, ErrorProcedure, ErrorLine, ErrorMessage, idAPICarga) 
        VALUES (NOW(), CURRENT_USER(), v_errno, NULL, NULL, 'sp_Upsert_DATA_Comentarios', NULL, v_message, IFNULL(idAPICarga, NULL)
);
        RESIGNAL;
    END;
    
in_guidCarga UNIQUEIDENTIFIER
AS
BEGIN
    -- Para pruebas
	-- DECLARE in_guidCarga AS UNIQUEIDENTIFIER
    -- SET in_guidCarga = '21ec2020-3aea-1069-a2dd-08002b30309d'

    -- Declarar variables
    DECLARE in_etiqueta NVARCHAR(100) 
    -- Declarar el cursor
    DECLARE etiqueta_cursor CURSOR FOR 
		SELECT DISTINCT etiqueta
		FROM STG_DATA_Comentarios
		WHERE guidCarga = in_guidCarga

    -- Abrir el cursor
    OPEN etiqueta_cursor

    -- Obtener la primera fila
    FETCH NEXT FROM etiqueta_cursor INTO in_etiqueta

    -- Iniciar el bucle
    WHILE @FETCH_STATUS = 0
    BEGIN
        -- Procesar los datos (en este ejemplo, solo los imprimimos) 
        SELECT 'guidCarga: ' AS _msg;+ cast (in_guidCarga as varchar(100)) 
        SELECT 'Etiqueta: ' AS _msg;+ in_etiqueta
        
        EXEC sp_Upsert_DATA_Comentarios_etiqueta in_guidCarga, in_etiqueta
	
        -- Obtener la siguiente fila
        FETCH NEXT FROM etiqueta_cursor INTO in_etiqueta
    END;

    -- Cerrar el cursor
    CLOSE etiqueta_cursor

    -- Liberar el cursor
    DEALLOCATE etiqueta_cursor

		
     --borrado de datos de staging
    DELETE 
    FROM STG_DATA_Comentarios
    WHERE guidCarga = in_guidCarga


END$$

CREATE PROCEDURE sp_Upsert_DATA_Comentarios_etiqueta (IN in_guidCarga CHAR(36), IN in_etiqueta VARCHAR, IN in_idAPICarga INT, IN in_idCompany INT, IN in_ejercicio INT, IN in_idCiclo INT, IN in_idFase INT, IN in_etiqueta VARCHAR(20), IN in_DeletedCount INT, IN in_InsertedCount INT, IN in_MergeOutput VARCHAR(255), IN in_guidCarga VARCHAR(255), IN in_idCompany VARCHAR(255), IN in_ejercicio VARCHAR(255), IN in_idCiclo VARCHAR(255), IN in_idFase VARCHAR(255), IN in_etiqueta VARCHAR(255), IN in_ROWCOUNT VARCHAR(255), IN in_guidCarga VARCHAR(255), IN in_idCompany VARCHAR(255), IN in_ejercicio VARCHAR(255), IN in_idCiclo VARCHAR(255), IN in_idFase VARCHAR(255), IN in_etiqueta VARCHAR(255), IN in_guidCarga INT, IN in_InsertedCount VARCHAR(255), IN in_DeletedCount VARCHAR(255), IN in_idAPICarga VARCHAR(255)) 
BEGIN

    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        DECLARE v_sqlstate CHAR(5);
        DECLARE v_errno INT;
        DECLARE v_message TEXT;
        GET DIAGNOSTICS CONDITION 1 v_sqlstate = RETURNED_SQLSTATE, v_errno = MYSQL_ERRNO, v_message = MESSAGE_TEXT;
        INSERT INTO errorlog (ErrorTime, UserName, ErrorNumber, ErrorSeverity, ErrorState, ErrorProcedure, ErrorLine, ErrorMessage, idAPICarga) 
        VALUES (NOW(), CURRENT_USER(), v_errno, NULL, NULL, 'sp_Upsert_DATA_Comentarios_etiqueta', NULL, v_message, IFNULL(idAPICarga, NULL)
);
        RESIGNAL;
    END;
    
in_guidCarga UNIQUEIDENTIFIER
	, in_etiqueta VARCHAR (100) 
AS
BEGIN
    

	
		-- borramos datos existentes para idCompany, in_ejercicio,in_idCiclo,in_idFase,in_etiqueta
		-- SOLO permite una combinacion
		DECLARE idAPICarga INT DEFAULT NULL;
		DECLARE idCompany INT DEFAULT NULL;
		DECLARE ejercicio INT DEFAULT NULL;
		DECLARE idCiclo INT DEFAULT NULL;
		DECLARE idFase INT DEFAULT NULL;
		--DECLARE in_etiqueta VARCHAR(20) 

		DECLARE DeletedCount INT DEFAULT NULL;
		DECLARE InsertedCount INT DEFAULT NULL;

		-- insercion de los datos en tabla final
		DECLARE in_MergeOutput TABLE (
			Action VARCHAR(10) 
		) 

		-- TODO: hacer bucle para cada etiqueta de la combinacion

		SELECT
			in_idAPICarga		= idAPICarga
			, in_idCompany	= idCompany
			, in_ejercicio	= ejercicio
			, in_idCiclo		= idCiclo
			, in_idFase		= idFase
			, in_etiqueta		= etiqueta
		FROM STG_DATA_Comentarios
		WHERE
			guidCarga = in_guidCarga
 LIMIT 1;			AND etiqueta = in_etiqueta

		-- borra todos los datos de la combinacion en la tabla final
		DELETE FROM DATA_Comentarios
		WHERE 
			idCompany		= in_idCompany	
			AND ejercicio	= in_ejercicio
			AND idCiclo	= in_idCiclo	
			AND idFase	= in_idFase	
			AND etiqueta	= in_etiqueta	
		
		SELECT in_DeletedCount = @in_ROWCOUNT 

		INSERT INTO DATA_Comentarios (
			idAPICarga, guidCarga, FechaUltModif, 
			idCompany, ejercicio, idCiclo, idFase, idEpigrafe,
			etiqueta, comentario
		) 
		SELECT 
			in_idAPICarga, in_guidCarga, NOW(), 
			in_idCompany, in_ejercicio, in_idCiclo, in_idFase, 
			idEpigrafe, in_etiqueta, comentario
		FROM 
			STG_DATA_Comentarios
		WHERE 
			guidCarga = in_guidCarga
			AND idCompany	= in_idCompany	
			AND ejercicio	= in_ejercicio
			AND idCiclo	= in_idCiclo	
			AND idFase	= in_idFase	
			AND etiqueta	= in_etiqueta	
		
		SELECT in_InsertedCount = @in_ROWCOUNT 
		
		-- -- borrado de datos de staging
		-- DELETE 
		-- FROM STG_DATA_Comentarios
		-- WHERE guidCarga = in_guidCarga

		SELECT 'Etiqueta: ' AS _msg;+ in_etiqueta;
		SELECT 'Número de filas insertadas: ' AS _msg;+ CAST(in_InsertedCount AS NVARCHAR(10)) 
		SELECT 'Número de filas actualizadas: ' AS _msg;+ CAST(in_DeletedCount AS NVARCHAR(10)) 

	
    
	BEGIN CATCH
        -- Insertar el error en la tabla de registro de errores
        INSERT INTO ErrorLog (
            ErrorTime, 
            UserName, 
            ErrorNumber, 
            ErrorSeverity, 
            ErrorState, 
            ErrorProcedure, 
            ErrorLine, 
            ErrorMessage,
			idAPICarga
        ) 
        VALUES (
            NOW(),
            CURRENT_USER(),
            ERROR_NUMBER(),
            ERROR_SEVERITY(),
            ERROR_STATE(),
            ERROR_PROCEDURE(),
            ERROR_LINE(),
            ERROR_MESSAGE(),
			in_idAPICarga
        ) 

        -- Re-lanzar el error para que sea visible al llamador
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Unhandled error'

END;



CREATE PROCEDURE sp_Upsert_DATA_Forecast (IN in_guidCarga CHAR(36), IN in_idAPICarga INT, IN in_idCompany INT, IN in_ejercicio INT, IN in_idCiclo INT, IN in_idFase INT, IN in_etiqueta VARCHAR(20), IN in_DeletedCount INT, IN in_InsertedCount INT, IN in_idCompany VARCHAR(255), IN in_ejercicio VARCHAR(255), IN in_idCiclo VARCHAR(255), IN in_idFase VARCHAR(255), IN in_guidCarga VARCHAR(255), IN in_ROWCOUNT VARCHAR(255), IN in_guidCarga VARCHAR(255), IN in_idCompany VARCHAR(255), IN in_ejercicio VARCHAR(255), IN in_idCiclo VARCHAR(255), IN in_idFase VARCHAR(255), IN in_guidCarga INT, IN in_InsertedCount VARCHAR(255), IN in_DeletedCount VARCHAR(255), IN in_idAPICarga VARCHAR(255)) 
BEGIN

    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        DECLARE v_sqlstate CHAR(5);
        DECLARE v_errno INT;
        DECLARE v_message TEXT;
        GET DIAGNOSTICS CONDITION 1 v_sqlstate = RETURNED_SQLSTATE, v_errno = MYSQL_ERRNO, v_message = MESSAGE_TEXT;
        INSERT INTO errorlog (ErrorTime, UserName, ErrorNumber, ErrorSeverity, ErrorState, ErrorProcedure, ErrorLine, ErrorMessage, idAPICarga) 
        VALUES (NOW(), CURRENT_USER(), v_errno, NULL, NULL, 'sp_Upsert_DATA_Forecast', NULL, v_message, IFNULL(idAPICarga, NULL)
);
        RESIGNAL;
    END;
    
in_guidCarga UNIQUEIDENTIFIER
AS
BEGIN
    

	

-- borramos datos existentes para idCompany, in_ejercicio,in_idCiclo,in_idFase,in_etiqueta
		-- SOLO permite una combinacion
		DECLARE idAPICarga INT DEFAULT NULL;
		DECLARE idCompany INT DEFAULT NULL;
		DECLARE ejercicio INT DEFAULT NULL;
		DECLARE idCiclo INT DEFAULT NULL;
		DECLARE idFase INT DEFAULT NULL;
		--DECLARE in_etiqueta VARCHAR(20) 

		DECLARE DeletedCount INT DEFAULT NULL;
		DECLARE InsertedCount INT DEFAULT NULL;

		-- seleccion de datos 
		SELECT
			in_idAPICarga		= idAPICarga
			, in_idCompany	= idCompany
			, in_ejercicio	= ejercicio
			, in_idCiclo		= idCiclo
			, in_idFase		= idFase
		FROM STG_DATA_Forecast
		WHERE
			guidCarga = in_guidCarga
 LIMIT 1;
		-- borra todos los datos de la combinacion en la tabla final
		DELETE FROM DATA_Forecast
		WHERE 
			idCompany		= in_idCompany	
			AND ejercicio	= in_ejercicio
			AND idCiclo	= in_idCiclo	
			AND idFase	= in_idFase	
			AND idEpigrafe IN (
				SELECT idepigrafe 
				FROM STG_DATA_Forecast 
				WHERE guidCarga = in_guidCarga 
				group by idEpigrafe
				) 
		SELECT in_DeletedCount = @in_ROWCOUNT 

		INSERT INTO DATA_Forecast (
			idAPICarga, guidCarga, FechaUltModif, 
			idCompany, ejercicio, idCiclo, idFase, 
			idCurrency, idEpigrafe,
			mes00, mes01, mes02, mes03, mes04, mes05, mes06,
			mes07, mes08, mes09, mes10, mes11, mes12, mes13
		) 
		SELECT 
			in_idAPICarga, in_guidCarga, NOW(), 
			in_idCompany, in_ejercicio, in_idCiclo, in_idFase, 
			idCurrency, idEpigrafe,
			mes00, mes01, mes02, mes03, mes04, mes05, mes06,
			mes07, mes08, mes09, mes10, mes11, mes12, mes13
		FROM 
			STG_DATA_Forecast
		WHERE 
			guidCarga = in_guidCarga
			AND idCompany	= in_idCompany	
			AND ejercicio	= in_ejercicio
			AND idCiclo	= in_idCiclo	
			AND idFase	= in_idFase	
		
		SELECT in_InsertedCount = @in_ROWCOUNT 

		-- borrado de datos de staging
		DELETE 
		FROM STG_DATA_Forecast
		WHERE guidCarga = in_guidCarga

		SELECT 'Número de filas insertadas: ' AS _msg;+ CAST(in_InsertedCount AS NVARCHAR(10)) 
		SELECT 'Número de filas actualizadas: ' AS _msg;+ CAST(in_DeletedCount AS NVARCHAR(10)) 

	
    
	BEGIN CATCH
        -- Insertar el error en la tabla de registro de errores
        INSERT INTO ErrorLog (
            ErrorTime, 
            UserName, 
            ErrorNumber, 
            ErrorSeverity, 
            ErrorState, 
            ErrorProcedure, 
            ErrorLine, 
            ErrorMessage,
			idAPICarga
        ) 
        VALUES (
            NOW(),
            CURRENT_USER(),
            ERROR_NUMBER(),
            ERROR_SEVERITY(),
            ERROR_STATE(),
            ERROR_PROCEDURE(),
            ERROR_LINE(),
            ERROR_MESSAGE(),
			in_idAPICarga
        ) 

        -- Re-lanzar el error para que sea visible al llamador
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Unhandled error'

END;


