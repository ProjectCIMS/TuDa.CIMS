﻿namespace TuDa.CIMS.Shared.Dtos;

public class CreateChemicalDto : CreateSubstanceDto
{
    /// <summary>
    /// The binding size of the chemical.
    /// </summary>
    public double BindingSize { get; set; }
}