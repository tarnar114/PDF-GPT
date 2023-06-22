import { PhoneIcon } from "@chakra-ui/icons";
import {
  Box,
  Flex,
  Input,
  InputLeftElement,
  InputGroup,
} from "@chakra-ui/react";
import React from "react";

export default function Chatbox() {
  return (
    <Flex h='100%' justify='center'>
      
      <InputGroup w='70%' alignSelf='end'mb={5} >
        <InputLeftElement pointerEvents="none">
          <PhoneIcon color="gray.300" />
        </InputLeftElement>
        <Input type="tel" placeholder="Phone number" />
      </InputGroup>
    </Flex>
  );
}
