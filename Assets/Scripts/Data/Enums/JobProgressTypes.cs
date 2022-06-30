using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum JobProgressTypes
{
    FIXED_AMOUNT, // Usually -> 0 (start) to 100 (target) progression.
    TARGET_ENTITY_ATTRIBUTE_PERCENT_OF_MAX, // Using a target attribute as the target progression. 
    SOURCE_ENTITY_ATTRIBUTE_PERCENT_OF_MAX, // Using a source attribute as the target progression. 
    TARGET_ENTITY_ATTRIBUTE_AMOUNT, // Using a target attribute as the target progression. 
    SOURCE_ENTITY_ATTRIBUTE_AMOUNT, // Using a source attribute as the target progression. 
}
