import React from "react";
import { Button, Flex, IconButton, useColorMode ,useColorModeValue} from "@chakra-ui/react";
import { MoonIcon,SunIcon } from "@chakra-ui/icons";
export default function Toggle() {
  const { colorMode, toggleColorMode } = useColorMode();

  return (
   <IconButton
        colorScheme={useColorModeValue('purple','orange')}
        icon={useColorModeValue(<MoonIcon />,<SunIcon />)}
        onClick={toggleColorMode}
    >
    </IconButton> 
  );
}
