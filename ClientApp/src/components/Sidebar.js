import { Box, Center, Text, Flex, useColorModeValue } from "@chakra-ui/react";
import React from "react";
import Toggle from "../theming/Toggle";
import Upload from "./Upload";

export default function Sidebar() {
  const bg = useColorModeValue("#FEEBC8", "#171923");
  return (
    <Flex h="100vh" w="20vh" bg={bg}alignItems='flex-end' >
      <Flex w='100%' marginBottom='5' justifyContent='space-around' >
        <Toggle />
        <Upload/>
      </Flex>
    </Flex>
  );
}
