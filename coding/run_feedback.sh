#!/usr/bin/env bash

# ============================================================
# Compiler Feedback Study
#
# Examples:
#
#   ./run_feedback.sh all neutral
#   ./run_feedback.sh all meowra
#   ./run_feedback.sh all raw
#
#   ./run_feedback.sh name_resolution neutral
#   ./run_feedback.sh type_use meowra
#   ./run_feedback.sh operator_use raw
# ============================================================


MODE="$1"
CONDITION="$2"


# ============================================================
# Print a GCC-style diagnostic
# ============================================================

gcc_style_error() {

    FILE="$1"
    LINE="$2"
    COLUMN="$3"
    SOURCE_LINE="$4"
    POINTER="$5"
    MESSAGE="$6"

    printf "%s:%s:%s: error: %s\n" \
        "$FILE" "$LINE" "$COLUMN" "$MESSAGE" >&2

    printf " %4s | %s\n" "$LINE" "$SOURCE_LINE" >&2
    printf "      | %s\n" "$POINTER" >&2
}


# ============================================================
# Run one scenario
# ============================================================

run_scenario() {

    SCENARIO="$1"
    CONDITION="$2"

    # --------------------------------------------------------
    # Scenario configuration
    # --------------------------------------------------------

    case "$SCENARIO" in

        statement_termination)

            FILE="statement_termination.c"
            LINE=4
            COLUMN=16
            SOURCE_LINE='    int cat = 5'
            POINTER='               ^'

            NEUTRAL_MESSAGE="This statement is missing a semicolon at the end. In C, statements like this need to end with a semicolon before the program can continue."

            MEOWRA_MESSAGE="Nya, this statement is missing a semicolon at the end. In C, statements like this need to end with a semicolon before the program can continue, meow!"
            ;;


        name_resolution)

            FILE="name_resolution.c"
            LINE=3
            COLUMN=5
            SOURCE_LINE='    cat = "meow";'
            POINTER='    ^~~'

            NEUTRAL_MESSAGE="The name 'cat' is being used without being declared first. Check that the variable has been declared before this statement tries to use it."

            MEOWRA_MESSAGE="Hmm, 'cat' is being used without being declared first, nya. Check that the variable has been declared before this statement tries to use it."
            ;;


        operator_use)

            FILE="operator_use.c"
            LINE=4
            COLUMN=11
            SOURCE_LINE='    int x - 5;'
            POINTER='          ^'

            NEUTRAL_MESSAGE="The operator between 'x' and '5' does not perform the intended assignment. Check which operator should be used when assigning a value to a variable."

            MEOWRA_MESSAGE="The operator between 'x' and '5' does not perform the intended assignment, nya. Check which operator should be used when assigning a value to a variable."
            ;;


        delimiter_matching)

            FILE="delimiter_matching.c"
            LINE=3
            COLUMN=10
            SOURCE_LINE='    if (1'
            POINTER='         ^'

            NEUTRAL_MESSAGE="The 'if' condition has an opening parenthesis without a matching closing parenthesis. Check the condition before the block begins and make sure the delimiters are balanced."

            MEOWRA_MESSAGE="Looks like the 'if' condition has an opening parenthesis without a matching closing parenthesis, nya. Check the condition before the block begins and make sure the delimiters are balanced."
            ;;


        function_call)

            FILE="function_call.c"
            LINE=10
            COLUMN=19
            SOURCE_LINE='    int y = add(5,"six");'
            POINTER='                  ^~~~~'

            NEUTRAL_MESSAGE="The second argument passed to 'add' does not match the type the function expects. Check the function parameters and make sure each argument uses the expected type."

            MEOWRA_MESSAGE="The second argument passed to 'add' does not match the type the function expects, nya. Check the function parameters and make sure each argument uses the expected type."
            ;;


        type_use)

            FILE="type_use.c"
            LINE=6
            COLUMN=9
            SOURCE_LINE='    x = "Meow";'
            POINTER='        ^~~~~~'

            NEUTRAL_MESSAGE="The value being assigned to 'x' does not match the variable's declared type. Check the type of 'x' and the type of the value being assigned to it."

            MEOWRA_MESSAGE="The value being assigned to 'x' does not match the variable's declared type, meow. Check the type of 'x' and the type of the value being assigned to it."
            ;;


        *)

            echo "Unknown scenario: $SCENARIO" >&2
            return 1
            ;;

    esac


    # --------------------------------------------------------
    # Make sure the C file exists
    # --------------------------------------------------------

    if [ ! -f "$FILE" ]; then

        echo "$FILE: error: source file not found" >&2
        return 1

    fi


    # --------------------------------------------------------
    # Actually run GCC
    # --------------------------------------------------------

    TEMP_ERROR=$(mktemp)
    TEMP_OUTPUT=$(mktemp)

    gcc "$FILE" -o "$TEMP_OUTPUT" 2>"$TEMP_ERROR"

    GCC_STATUS=$?


    # --------------------------------------------------------
    # Compilation unexpectedly succeeded
    # --------------------------------------------------------

    if [ "$GCC_STATUS" -eq 0 ]; then

        printf "%s: compilation succeeded; expected an error for this stimulus.\n" \
            "$FILE" >&2

        rm -f "$TEMP_ERROR" "$TEMP_OUTPUT"

        return 0

    fi


    # --------------------------------------------------------
    # RAW CONDITION
    #
    # Show GCC's real stderr exactly as GCC produced it.
    # --------------------------------------------------------

    if [ "$CONDITION" = "raw" ]; then

        cat "$TEMP_ERROR"

        rm -f "$TEMP_ERROR" "$TEMP_OUTPUT"

        return "$GCC_STATUS"

    fi


    # --------------------------------------------------------
    # NEUTRAL CONDITION
    # --------------------------------------------------------

    if [ "$CONDITION" = "neutral" ]; then

        gcc_style_error \
            "$FILE" \
            "$LINE" \
            "$COLUMN" \
            "$SOURCE_LINE" \
            "$POINTER" \
            "$NEUTRAL_MESSAGE"

    # --------------------------------------------------------
    # MEOWRA CONDITION
    # --------------------------------------------------------

    elif [ "$CONDITION" = "meowra" ]; then

        gcc_style_error \
            "$FILE" \
            "$LINE" \
            "$COLUMN" \
            "$SOURCE_LINE" \
            "$POINTER" \
            "$MEOWRA_MESSAGE"

    else

        echo "Unknown condition: $CONDITION" >&2

        rm -f "$TEMP_ERROR" "$TEMP_OUTPUT"

        return 1

    fi


    rm -f "$TEMP_ERROR" "$TEMP_OUTPUT"

    return "$GCC_STATUS"
}


# ============================================================
# All experimental scenarios
# ============================================================

SCENARIOS=(
    statement_termination
    name_resolution
    operator_use
    delimiter_matching
    function_call
    type_use
)


# ============================================================
# Run all scenarios
# ============================================================

if [ "$MODE" = "all" ]; then

    if [ "$CONDITION" != "raw" ] &&
       [ "$CONDITION" != "neutral" ] &&
       [ "$CONDITION" != "meowra" ]; then

        echo "Usage:"
        echo "  $0 all raw"
        echo "  $0 all neutral"
        echo "  $0 all meowra"

        exit 1
    fi


    for scenario in "${SCENARIOS[@]}"; do

        run_scenario "$scenario" "$CONDITION"

        printf "\n"

    done

    exit 0
fi


# ============================================================
# Run one scenario
# ============================================================

if [ "$CONDITION" = "raw" ] ||
   [ "$CONDITION" = "neutral" ] ||
   [ "$CONDITION" = "meowra" ]; then

    run_scenario "$MODE" "$CONDITION"

    exit $?
fi


# ============================================================
# Help
# ============================================================

echo "Usage:"
echo ""
echo "Single scenario:"
echo "  $0 name_resolution raw"
echo "  $0 name_resolution neutral"
echo "  $0 name_resolution meowra"
echo ""
echo "All scenarios:"
echo "  $0 all raw"
echo "  $0 all neutral"
echo "  $0 all meowra"

exit 1